using System;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Exceptions;
using CommonLibraries.MediaFolders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NotificationsData;
using NotificationsData.Main;
using NotificationsServer.Services;
using NotificationsServer.WebSockets;
using NotificationsServer.WebSockets.WebSocketsExceptions;

namespace NotificationsServer
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyOrigin().AllowAnyHeader()
          .AllowAnyMethod());
      });
      services.AddDbContext<TwoButtonsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TwoButtonsConnection")));
    

      services.AddSingleton<NotificationManager>();
      services.AddTransient<NotificationsDataUnitOfWork>();
       services.AddTransient<INotificationsMessageService, NotificationsMessageService>();
      // services.AddTransient<IVkService, VkService>();
      // services.AddTransient<IFbService, FbService>();

      services.AddTransient<WebSocketsController>();
      
     // services.AddSingleton<WebSocketManager>();
      //services.AddWebSocketManager();

      services.AddOptions();
      services.AddConnectionsHub();
      services.Configure<ServersSettings>(Configuration.GetSection("ServersSettings"));
      services.Configure<MediaConverterSettings>(Configuration.GetSection("MediaConverterSettings"));
      services.AddSingleton<MediaConverter>();
      var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtSettings));
      var secretKey = jwtAppSettingOptions["SecretKey"];
      var issuer = jwtAppSettingOptions[nameof(JwtSettings.Issuer)];
      var audience = jwtAppSettingOptions[nameof(JwtSettings.Audience)];

      services.Configure<JwtSettings>(options =>
      {
        options.Issuer = issuer;
        options.Audience = audience;
        options.SigningCredentials = new SigningCredentials(JwtSettings.CreateSecurityKey(secretKey), SecurityAlgorithms.HmacSha256);
      });

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(configureOptions =>
      {
        configureOptions.ClaimsIssuer = issuer;
        configureOptions.RequireHttpsMetadata = false;
        configureOptions.TokenValidationParameters = JwtSettings.CreateTokenValidationParameters(issuer, audience, JwtSettings.CreateSecurityKey(secretKey));
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseExceptionHandling();
      app.UseWebScoketExceptionHandling();
      app.UseDefaultFiles();
      app.UseStaticFiles();
      app.UseWebSockets();
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });
      app.UseAuthentication();
      app.UseWebSockets();
      app.UseMvc();
     // serviceProvider.GetService<WebSocketManager>();
      app.MapWebSocketManager("/ws");
    }
  }
}

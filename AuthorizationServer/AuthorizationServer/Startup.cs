using System.Globalization;
using System.Net;
using AuthorizationData;
using AuthorizationData.Account;
using AuthorizationData.Main;
using AuthorizationServer.Extensions;
using AuthorizationServer.Infrastructure.EmailJwt;
using AuthorizationServer.Infrastructure.Jwt;
using AuthorizationServer.Infrastructure.Services;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Exceptions;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationServer
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
     
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<TwoButtonsContext>(
        options => options.UseSqlServer(Configuration.GetConnectionString("TwoButtonsConnection")));
      services.AddDbContext<TwoButtonsAccountContext>(
        options => options.UseSqlServer(Configuration.GetConnectionString("TwoButtonsAccountConnection")));

      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyOrigin().AllowAnyHeader()
          .AllowAnyMethod());
      });

      services.AddOptions();
      services.AddConnectionsHub();
      services.Configure<ServersSettings>(Configuration.GetSection("ServersSettings"));

      services.Configure<FacebookAuthSettings>(Configuration.GetSection(nameof(FacebookAuthSettings)));
      services.Configure<VkAuthSettings>(Configuration.GetSection(nameof(VkAuthSettings)));
      services.Configure<VkAuthSettingsTest>(Configuration.GetSection(nameof(VkAuthSettingsTest)));

      var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtSettings));
      var secretKey = jwtAppSettingOptions["SecretKey"];
      var issuer = jwtAppSettingOptions[nameof(JwtSettings.Issuer)];
      var audience = jwtAppSettingOptions[nameof(JwtSettings.Audience)];

      var key = JwtSettings.CreateSecurityKey(secretKey);

      services.Configure<JwtSettings>(options =>
      {
        options.Issuer = issuer;
        options.Audience = audience;
        options.SigningCredentials = new SigningCredentials(key,
          SecurityAlgorithms.HmacSha256);
      });

      var emailJwtAppSettingOptions = Configuration.GetSection(nameof(EmailJwtSettings));
      var emailSecretKey = emailJwtAppSettingOptions["SecretKey"];
      var emailIssuer = emailJwtAppSettingOptions[nameof(EmailJwtSettings.Issuer)];
      var emailAudience = emailJwtAppSettingOptions[nameof(EmailJwtSettings.Audience)];
      var codeAudience = emailJwtAppSettingOptions[nameof(EmailJwtSettings.Code)];

      services.Configure<EmailJwtSettings>(options =>
      {
        var emailKey = JwtSettings.CreateSecurityKey(emailSecretKey);
        options.Issuer = emailIssuer;
        options.Audience = emailAudience;
        options.Code = codeAudience;
        options.SymmetricSecurityKey = emailKey;
        options.TokenValidationParameters = JwtSettings.CreateTokenValidationParameters(issuer, audience, emailKey);
        options.SigningCredentials = new SigningCredentials(emailKey, SecurityAlgorithms.HmacSha256);
      });

      services.AddSingleton<IJwtService, JwtService>();
      services.AddSingleton<IEmailJwtService, EmailJwtService>();
      services.AddTransient<IVkService, VkService>();
      services.AddTransient<IFbService, FbService>();
      services.AddTransient<ICommonAuthService, CommonAuthService>();
      services.AddTransient<IInternalAuthService, InternalAuthService>();
      services.AddTransient<IExternalAuthService, ExternalAuthService>();
      services.AddTransient<AuthorizationUnitOfWork>();

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(configureOptions =>
      {
        configureOptions.ClaimsIssuer = issuer;
        configureOptions.RequireHttpsMetadata = false;
        configureOptions.TokenValidationParameters =
          JwtSettings.CreateTokenValidationParameters(issuer, audience, key);
      });

      services.AddLocalization(options => options.ResourcesPath = "Resources");

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();
      app.UseExceptionHandling();

      if (env.IsDevelopment())
      {
        //app.UseDeveloperExceptionPage();
        //app.UseDatabaseErrorPage();
        //app.UseBrowserUrl();
      }

      
      var supportedCultures = new[]
      {
        new CultureInfo("en-US"),
        new CultureInfo("en-GB"),
        new CultureInfo("en"),
        new CultureInfo("ru-RU"),
        new CultureInfo("ru"),
        new CultureInfo("de-DE"),
        new CultureInfo("de")
      };
      app.UseRequestLocalization(new RequestLocalizationOptions
      {
        DefaultRequestCulture = new RequestCulture("ru-RU"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
      });

      app.UseDefaultFiles();
      app.UseStaticFiles(new StaticFileOptions
      {
        OnPrepareResponse = ctx => { ctx.Context.Response.Headers.Add("Cache-Control", "no-store"); }
      });

      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });
      app.UseAuthentication();
      app.UseMvc();
    }
  }
}
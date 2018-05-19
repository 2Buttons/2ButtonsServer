using System;
using System.Linq;
using System.Net;
using System.Text;
using AccountServer.Auth;
using AccountServer.Extensions;
using AccountServer.Models;
using AccountServer.Models.Facebook;
using AccountServer.Models.Vk;
using AccountServer.Services;
using AccountServer.ViewModels.InputParameters.Auth;
using CommonLibraries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TwoButtonsAccountDatabase;
using TwoButtonsAccountDatabase.Repostirories;
using TwoButtonsDatabase;

namespace AccountServer
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<TwoButtonsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TwoButtonsConnection")));
      services.AddDbContext<TwoButtonsAccountContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TwoButtonsAccountConnection")));

      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyOrigin().AllowAnyHeader()
          .AllowAnyMethod());
      });

      services.AddOptions();

      services.Configure<FacebookAuthSettings>(Configuration.GetSection(nameof(FacebookAuthSettings)));
      services.Configure<VkAuthSettings>(Configuration.GetSection(nameof(VkAuthSettings)));

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

      services.AddSingleton<IJwtService, JwtService>();
      services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
      services.AddTransient<AccountUnitOfWork>(); // Attention!! TODO maybe scoped
      services.AddTransient<TwoButtonsUnitOfWork>();

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

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
        app.UseBrowserLink();
      }

      // обработка ошибок HTTP
      app.UseExceptionHandler(
        builder =>
        {
          builder.Run(
            async context =>
            {
              context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
              context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

              var error = context.Features.Get<IExceptionHandlerFeature>();
              if (error != null)
              {
                context.Response.AddApplicationError(error.Error.Message);
                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
              }
            });
        });

      // app.UseStatusCodePages();

      app.UseDefaultFiles();
      app.UseStaticFiles(new StaticFileOptions()
      {
        OnPrepareResponse = ctx =>
        {
          ctx.Context.Response.Headers.Add("Cache-Control", "no-store");
        }
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

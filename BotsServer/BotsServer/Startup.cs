﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotsData;
using BotsData.Contexts;
using BotsServer.Jobs;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BotsServer
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      });
      services.AddDbContext<TwoButtonsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TwoButtonsConnection")));
      services.AddDbContext<TwoButtonsAccountContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TwoButtonsAccountConnection")));
      services.Configure<ConnectionStrings>(options => options.TwoButtonsConnection = Configuration.GetConnectionString("TwoButtonsConnection"));
      services.AddTransient<BotsUnitOfWork>();
      services.AddSingleton<BotsManager>();
      services.AddOptions();
      services.Configure<ServersSettings>(Configuration.GetSection("ServersSettings"));
      services.AddConnectionsHub();


      var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtSettings));
      var secretKey = jwtAppSettingOptions["SecretKey"];
      var issuer = jwtAppSettingOptions[nameof(JwtSettings.Issuer)];
      var audience = jwtAppSettingOptions[nameof(JwtSettings.Audience)];

      services.Configure<JwtSettings>(options =>
      {
        options.Issuer = issuer;
        options.Audience = audience;
        options.SigningCredentials = new SigningCredentials(JwtSettings.CreateSecurityKey(secretKey),
          SecurityAlgorithms.HmacSha256);
      });

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(configureOptions =>
      {
        configureOptions.ClaimsIssuer = issuer;
        configureOptions.RequireHttpsMetadata = false;
        configureOptions.TokenValidationParameters =
          JwtSettings.CreateTokenValidationParameters(issuer, audience, JwtSettings.CreateSecurityKey(secretKey));
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      if (env.IsDevelopment())
      {
        loggerFactory.AddConsole();
        loggerFactory.AddDebug();
      }
      loggerFactory.AddDebug();
      app.UseExceptionHandling();

      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      app.UseAuthentication();
      app.UseMvc();
    }
  }
}

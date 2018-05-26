﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialData;
using SocialData.Account;
using SocialData.Main;

namespace SocialServer
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
        services.AddDbContext<TwoButtonsAccountContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TwoButtonsAccountConnection")));

      services.AddTransient<SocialDataUnitOfWork>();

        services.AddOptions();
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

      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
      {
        loggerFactory.AddConsole(Configuration.GetSection("Logging"));
        loggerFactory.AddDebug();

        if (env.IsDevelopment())
        {
          app.UseDeveloperExceptionPage();
        }

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
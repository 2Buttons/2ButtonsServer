using System;
using System.Net;
using System.Text;
using AccountServer.Auth;
using AccountServer.Extensions;
using AccountServer.Models;
using AccountServer.Models.Facebook;
using AccountServer.Models.Vk;
using AccountServer.ViewModels.InputParameters.Auth;
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

      services.AddSingleton<IJwtFactory, JwtFactory>();
      services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

      services.Configure<FacebookAuthSettings>(Configuration.GetSection(nameof(FacebookAuthSettings)));
      services.Configure<VkAuthSettings>(Configuration.GetSection(nameof(VkAuthSettings)));

      var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
      var symmetricKeyAsBase64 = jwtAppSettingOptions["SecretKey"];
      var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
      var signingKey = new SymmetricSecurityKey(keyByteArray);

      services.Configure<JwtIssuerOptions>(options=>
      {
        options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
        options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
        options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
      });

      //   services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AccountContext>();

      services.AddTransient<AccountUnitOfWork>(); // Attention!! TODO maybe scoped

      var tokenValidationParameters = new TokenValidationParameters
      {
        // укзывает, будет ли валидироваться издатель при валидации токена
        ValidateIssuer = true,
        // строка, представляющая издателя
        ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

        // будет ли валидироваться потребитель токена
        ValidateAudience = true,
        // установка потребителя токена
        ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
        

        // установка ключа безопасности
        IssuerSigningKey = signingKey,
        // валидация ключа безопасности
        ValidateIssuerSigningKey = true,

        // будет ли валидироваться время существования
        ValidateLifetime = true,
       // RequireExpirationTime = false,
        ClockSkew = TimeSpan.Zero
      };

      services.AddAuthentication(options=>
        {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(configureOptions =>
        {
          configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
          configureOptions.RequireHttpsMetadata = false;
          configureOptions.TokenValidationParameters = tokenValidationParameters;
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

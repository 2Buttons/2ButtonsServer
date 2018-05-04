using System;
using System.Text;
using AccountServer.Entities;
using AccountServer.Models;
using AccountServer.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
      services.AddMvc();
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyOrigin().AllowAnyHeader()
                  .AllowAnyMethod());
      });

      services.AddOptions();
      services.Configure<AuthenticationOptions>(Configuration.GetSection("AuthenticationOptions"));
      var authenticationOptions = Configuration.GetSection("AuthenticationOptions");
      var symmetricKeyAsBase64 = authenticationOptions["SecretKey"];
      var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
      var signingKey = new SymmetricSecurityKey(keyByteArray);


      services.AddDbContext<TwoButtonsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TwoButtonsDatabase")));
      services.AddDbContext<AccountContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TwoButtonsAccountDatabase")));
      //   services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AccountContext>();

      services.AddTransient<AuthenticationRepository>();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.RequireHttpsMetadata = false;
          options.TokenValidationParameters = new TokenValidationParameters
          {
            // укзывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = authenticationOptions["Issuer"],

            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = authenticationOptions["Audience"],
            // будет ли валидироваться время существования
            ValidateLifetime = true,

            // установка ключа безопасности
            IssuerSigningKey = signingKey,
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
          };
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

      app.UseAuthentication();

      app.UseMvc();

    }
  }
}

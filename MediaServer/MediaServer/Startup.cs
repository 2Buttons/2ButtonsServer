using System;
using System.IO;
using System.Net.Mime;
using CommonLibraries;
using MediaServer.FileSystem;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TwoButtonsDatabase;

namespace MediaServer
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
      var connection = Configuration.GetConnectionString("TwoButtonsDatabase");
      services.AddDbContext<TwoButtonsContext>(options => options.UseSqlServer(connection));
      services.AddTransient<TwoButtonsUnitOfWork>();

      services.AddOptions();
      services.Configure<MediaData>(Configuration.GetSection("MediaData"));

      services.AddMvc();
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyOrigin().AllowAnyHeader()
                  .AllowAnyMethod());
      });
      services.AddSingleton<IFileManager, FileManager>();

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

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<MediaData> mediaOptions)
    {

      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      app.UseStaticFiles(new StaticFileOptions
      {
        FileProvider = new PhysicalFileProvider(
              Path.Combine(AppDomain.CurrentDomain.BaseDirectory, mediaOptions.Value.RootFolderRelativePath, mediaOptions.Value.RootFolderName)),
        RequestPath = "/images"
      });

      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      app.UseMvc();
    }

  }
}
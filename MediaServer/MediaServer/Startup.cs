using System;
using System.IO;
using System.Net.Mime;
using MediaServer.FileSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
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

            services.AddOptions();
            services.Configure<MediaData>(Configuration.GetSection("MediaData"));

            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyOrigin().AllowAnyHeader()
                    .AllowAnyMethod());
            });
            services.AddSingleton<IFileManager, FileManager>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<MediaData> mediaOptions)
        {

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory,mediaOptions.Value.RootFolderRelativePath, mediaOptions.Value.RootFolderName)),
                RequestPath = "/images"
            });

          //app.UseForwardedHeaders(new ForwardedHeadersOptions
          //{
          //  ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
          //});

      app.UseMvc();
        }

    }
}
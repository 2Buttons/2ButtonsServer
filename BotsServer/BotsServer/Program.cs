﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.CommandLine;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace BotsServer
{
  public class Program
  {
    public static IConfiguration Configuration { get; private set; }
    public static string Scheme { get; } = "http";
    public static string IpAddress { get; } = "localhost";
    public static string Port { get; private set; }
    public static string Url => Scheme + "://" + IpAddress + ":" + Port;

    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args)
    {
      var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory);

      builder.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
      builder.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "..", "routingsettings.json"));
      builder.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "..", "hiddensettings.json"));

      Configuration = builder.Build();
      Port = Configuration["WebHost:Port"];

      var commandLine = Command.WithName("host").HasOption("-port", "-p");

      if (commandLine.TryParse(args, out var command) && command is HostCommand hostCommand &&
          !hostCommand.Port.IsNullOrEmpty()) Port = hostCommand.Port;

      return WebHost.CreateDefaultBuilder(args).UseConfiguration(Configuration).UseUrls(Url)
        .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Trace)).UseStartup<Startup>().UseSerilog(
          (ctx, cfg) =>
          {
            var serverName = Configuration.GetValue<string>("ServerName");
            var path = Path.Combine(AppContext.BaseDirectory, ".." + Path.DirectorySeparatorChar, "Logs",
              serverName + "{Date}" + ".log");

            cfg.ReadFrom.Configuration(ctx.Configuration).MinimumLevel.Debug().MinimumLevel
              .Override("Microsoft", LogEventLevel.Information).WriteTo.RollingFile(path,
                outputTemplate:
                "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Application} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}");//,fileSizeLimitBytes:500*1024*1024);
          }).Build();
    }
  }
}

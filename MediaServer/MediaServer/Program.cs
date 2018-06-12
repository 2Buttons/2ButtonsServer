using System;
using System.IO;
using CommonLibraries.CommandLine;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace MediaServer
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
      var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json");

      Configuration = builder.Build();

      if (!Command.WithName("host").HasOption("-port", "-p").TryParse(args, out var command))
      {
        if (command is HostCommand hostCommand && !hostCommand.Port.IsNullOrEmpty()) Port = hostCommand.Port;
      }
      else
      {
        Port = Configuration["WebHost:Port"];
      }
      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args)
    {
      return WebHost.CreateDefaultBuilder(args).UseUrls(Url)
        .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Trace)).UseStartup<Startup>().UseSerilog(
          (ctx, cfg) =>
          {
            var serverName = Configuration.GetValue<string>("ServerName");
            var path = Path.Combine(AppContext.BaseDirectory, ".." + Path.DirectorySeparatorChar, "Logs",
              serverName + "{Date}-" + ".log");

            cfg.ReadFrom.Configuration(ctx.Configuration).MinimumLevel.Debug().MinimumLevel
              .Override("Microsoft", LogEventLevel.Information).WriteTo.RollingFile(path,
                outputTemplate:
                "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Application} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}");
          }).Build();
    }
  }
}
﻿using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace NotificationServer
{
    public class Program
    {
    public static string Scheme { get; private set; }
      public static string IpAddress { get; private set; }
      public static string Port { get; private set; }
      public static string Url => Scheme + IpAddress + ":" + Port;


      public static void Main(string[] args)
      {
        var builder = new ConfigurationBuilder()
          .SetBasePath(AppContext.BaseDirectory)
          .AddJsonFile("appsettings.json");

        var configuration = builder.Build();
        var webHost = configuration.GetSection("WebHost");
        Scheme = webHost[nameof(Scheme)];
        IpAddress = webHost[nameof(IpAddress)];
        Port = webHost[nameof(Port)];

        BuildWebHost(args).Run();
      }

      public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
          .UseUrls(Url)
          .UseStartup<Startup>()
          .Build();
  }
}

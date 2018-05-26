﻿using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace QuestionsServer
{
    public class Program
    {
        public static IConfiguration Configuration { get; private set; }
        public static string Scheme { get; private set; }
        public static string IpAddress { get; private set; }
        public static string Port { get; private set; }
        public static string Url => Scheme + IpAddress + ":" + Port;


        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            Scheme = Configuration["WebHost:Scheme"];
            IpAddress = Configuration["WebHost:IPAddress"];
            Port = Configuration["WebHost:Port"];

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(Url)
                .UseStartup<Startup>()
                .Build();
    }
}

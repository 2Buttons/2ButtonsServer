﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthorizedServer
{
    public class Program
    {
        public static IConfiguration Configuration { get; private set; }
        public static string Scheme { get; private set; }
        public static string IPAddress { get; private set; }
        public static string Port { get; private set; }
        public static string Url => Scheme + IPAddress + ":" + Port;


        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            Scheme = Configuration["WebHost:Scheme"];
            IPAddress = Configuration["WebHost:IPAddress"];
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

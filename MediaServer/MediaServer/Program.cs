using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MediaServer
{
    public class Program
    {
        public static IConfiguration Configuration { get; private set; }
        public static string Protocol { get; private set; }
        public static string IPAddress { get; private set; }
        public static string Port { get; private set; }
        public static string Url => Protocol + IPAddress + ":" + Port;


        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            Protocol = Configuration["WebHost:Protocol"];
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

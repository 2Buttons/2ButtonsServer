using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationsServer.WebSockets
{
    public static class ServiceCollectionExtension
    {
      public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
      {
        services.AddTransient<WebSocketConnectionManager>();
      foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
        {
          if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
          {
            services.AddSingleton(type);
          }
        }

        return services;
    }
    }
}

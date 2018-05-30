using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibraries.WebSockets
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

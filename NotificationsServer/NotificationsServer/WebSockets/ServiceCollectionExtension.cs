//using System.Reflection;
//using CommonLibraries.WebSockets;
//using Microsoft.Extensions.DependencyInjection;

//namespace NotificationServer.WebSockets
//{
//    public static class ServiceCollectionExtension
//    {
//      public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
//      {
//        services.AddTransient<WebSocketConnectionManager>();
//      foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
//        {
//         // if (type.GetTypeInfo().BaseType == typeof(WebSocketManager))
//         // {
//            services.AddSingleton(type);
//          //}
//        }

//        return services;
//    }
//    }
//}

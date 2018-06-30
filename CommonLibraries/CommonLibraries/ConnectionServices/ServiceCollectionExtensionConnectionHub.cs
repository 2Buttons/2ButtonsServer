using Microsoft.Extensions.DependencyInjection;

namespace CommonLibraries.ConnectionServices
{
  public static class ServiceCollectionExtensionConnectionHub
  {
    public static void AddConnectionsHub(this IServiceCollection services)
    {
      services.AddSingleton<MediaServerConnection>();
      services.AddSingleton<MonitoringServerConnection>();
      services.AddSingleton<NotificationsServerConnection>();
      services.AddSingleton<ConnectionsHub>();
    }
  }
}
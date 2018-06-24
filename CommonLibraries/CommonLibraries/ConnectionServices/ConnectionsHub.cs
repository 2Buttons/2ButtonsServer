using CommonLibraries.Helpers;

namespace CommonLibraries.ConnectionServices
{
  public class ConnectionsHub
  {
    public MediaServerConnection Media { get; }
    public MonitoringServerConnection Monitoring { get; }
    public NotificationsServerConnection Notifications { get; }

    public ConnectionsHub(MediaServerConnection mediaServerConnection,
      MonitoringServerConnection monitoringServerConnection,
      NotificationsServerConnection notificationsServerConnection)
    {
      Media = mediaServerConnection;
      Monitoring = monitoringServerConnection;
      Notifications = notificationsServerConnection;
    }
  }
}
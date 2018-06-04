using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NotificationsData;
using NotificationsData.Account;
using NotificationsData.Main;
using NotificationsData.Main.Entities;

namespace NotificationServer.WebSockets
{
  public class WebSocketConnectionManager
  {
    private readonly List<SocketPair> _sockets = new List<SocketPair>();

    public event Action<int> AddNewConnection;
    NotificationsDataUnitOfWork _db;
    private List<NotificationDb> notifications;

    public WebSocketConnectionManager(NotificationsDataUnitOfWork db)
    {
      _db = db;
      //notifications = _db.Notifications.GetNotifications(1).GetAwaiter().GetResult();
    }

    public virtual async Task OnConnected(int userId, WebSocket socket)
    {
      // var isFirstAdded = _sockets.All(x => x.UserId != userId);
      // _sockets.Add(new SocketPair(userId, socket));
      string message = "Hello";
     
        notifications =  _db.Notifications.GetNotifications(userId);
        var t = notifications;
        message = JsonConvert.SerializeObject(t);
  
      await SendMessageAsync(socket, JsonConvert.SerializeObject(notifications));
      //  if (isFirstAdded) await SendNotificationForNewConnection(userId);
    }

    public virtual async Task OnDisconnected(WebSocket socket)
    {
      var socketInList = _sockets.FirstOrDefault(x => x.WebSocket == socket);
      if (socketInList == null) return;
      _sockets.Remove(socketInList);
    }

    public async Task SendMessageAsync(WebSocket socket, string message)
    {
      
      if (socket.State != WebSocketState.Open) return;
      var bytes = Encoding.UTF8.GetBytes(message);
      await socket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true,
        CancellationToken.None);
    }

    public async Task SendMessageAsync(IEnumerable<WebSocket> sockets, string message)
    {
      foreach (var socket in sockets) await SendMessageAsync(socket, message);
    }

    public async Task SendNotificationAsync(int userId, string notification)
    {
     
      await SendMessageAsync(_sockets.Where(x => x.UserId == userId).Select(x => x.WebSocket).ToList(), notification);
    }

    public async Task SendMessageToAllAsync(string notification)
    {
      await SendMessageAsync(_sockets.Select(x => x.WebSocket).ToList(), notification);
    }

    public async Task SendNotificationForNewConnection(int userId)
    {
      //foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
      //{
      //  if (type.GetTypeInfo().BaseType == typeof(NotificationsDataUnitOfWork))
      //  {

     
     // var notifications = await _db.Notifications.GetNotifications(userId);
    //  await SendNotificationAsync(userId, JsonConvert.SerializeObject(notifications));
      return;
      //  }
      //}


    }
  }
}
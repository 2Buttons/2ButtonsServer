using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using CommonLibraries.WebSockets;

namespace AccountServer.Infrastructure.Services
{
    public class NotificationsMessageHandler : WebSocketHandler
    {
      public NotificationsMessageHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
      {
      }

      public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
      {
        throw new NotImplementedException();
      }
    }
}

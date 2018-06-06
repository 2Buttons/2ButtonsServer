using System;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace NotificationServer.WebSockets
{
  public class WebSocketManagerMiddleware
  {
    private readonly RequestDelegate _next;

    public WebSocketManagerMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context, WebSocketsController controller)
    {
      if (!context.WebSockets.IsWebSocketRequest)
      {
        await _next.Invoke(context);
        return;
      }
      var socket = await context.WebSockets.AcceptWebSocketAsync();
      var id = ExtractUserIdFromContext(context);
      if (id <= 0) throw new Exception("Guest does not have a permission to get notifications.");
      await controller.OnConnected(id, socket);

      await Receive(socket, async (result, buffer) =>
      {
        switch (result.MessageType)
        {
          case WebSocketMessageType.Binary:
          case WebSocketMessageType.Text:
            // await _webSocketHandler.ReceiveAsync(socket, result, buffer);
            return;
          case WebSocketMessageType.Close:
            await controller.OnDisconnected(socket);
            return;
        }
      });

      await _next.Invoke(context);
    }

    private int ExtractUserIdFromContext(HttpContext context)
    {
      AuthenticateResult authenticateResult =
         context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme).GetAwaiter().GetResult();//.AuthenticateAsync(OAuthValidationDefaults.AuthenticationScheme);
      //authenticateResult.Principal.Claims
      //var claims = context.User?.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
      //if (string.IsNullOrEmpty(claims)) return -1;
      //var isUserIdFromAuth =
      //  int.TryParse(claims, out var userId);
      //if (isUserIdFromAuth) return userId;
      //return -1;
      var claims = context.User?.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);//authenticateResult.Principal.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
      if (claims == null) return -1;
      var isUserIdFromAuth =
        int.TryParse(claims, out var userId);
      if (isUserIdFromAuth) return userId;
      return -1;
    }

    private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
    {
      var buffer = new byte[1024 * 4];
      while (socket.State == WebSocketState.Open)
      {
        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        handleMessage(result, buffer);
      }
    }
  }
}
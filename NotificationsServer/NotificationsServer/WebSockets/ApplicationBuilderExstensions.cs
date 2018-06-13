using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace NotificationsServer.WebSockets
{
  public static class ApplicationBuilderExstensions
  {
    public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app, PathString path)
    {
      if (app == null) throw new ArgumentNullException(nameof(app));
      return app.Map(path, _app => _app.UseMiddleware<WebSocketManagerMiddleware>());
    }
  }
}
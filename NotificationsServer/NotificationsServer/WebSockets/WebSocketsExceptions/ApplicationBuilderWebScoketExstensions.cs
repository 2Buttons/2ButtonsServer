using System;
using Microsoft.AspNetCore.Builder;

namespace NotificationServer.WebSockets.WebSocketsExceptions
{
  public static class ApplicationBuilderWebScoketExstensions
  {
    public static IApplicationBuilder UseWebScoketExceptionHandling(this IApplicationBuilder app)
    {
      if (app == null) throw new ArgumentNullException(nameof(app));
      return app.UseMiddleware<WebSocketExceptionHandlingMiddleware>();
    }
  }
}
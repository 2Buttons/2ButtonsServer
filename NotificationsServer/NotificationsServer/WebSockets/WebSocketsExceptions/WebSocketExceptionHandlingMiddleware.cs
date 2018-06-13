using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace NotificationsServer.WebSockets.WebSocketsExceptions
{
  public class WebSocketExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    //private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public WebSocketExceptionHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
      //  _logger = logger;
    }

    public async Task Invoke(HttpContext context, IHostingEnvironment env, ILoggerFactory loggerFactory,
      NotificationManager notification)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        // _logger.LogError(EventIds.GlobalException, ex, ex.Message);
        if (context.WebSockets.IsWebSocketRequest) HandleExceptionAsync(context, loggerFactory, notification, ex);
        throw;
      }
    }

    private static void HandleExceptionAsync(HttpContext context, ILoggerFactory loggerFactory,
      NotificationManager notificationMananger, Exception exception)
    {
      notificationMananger.CleanUpSockets();
    }
  }
}
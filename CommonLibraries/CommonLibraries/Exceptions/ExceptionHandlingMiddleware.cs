using System;
using System.Threading.Tasks;
using CommonLibraries.Exceptions.ApiExceptions;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommonLibraries.Exceptions
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    //private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
      //  _logger = logger;
    }

    public async Task Invoke(HttpContext context, IHostingEnvironment env,
      ILoggerFactory loggerFactory /* other dependencies */)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        // _logger.LogError(EventIds.GlobalException, ex, ex.Message);
        await HandleExceptionAsync(context, loggerFactory, ex);
        throw;
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, ILoggerFactory loggerFactory, Exception exception)
    {
      if(context.Response.StatusCode == 200) context.Response.StatusCode = 500;

      switch (exception)
      {
        case NotFoundException _:
          context.Response.StatusCode = 404;
          break;
      }
      var response =
        new ResponseObject(context.Response.StatusCode, null, null)
        {
          Message = exception.Message,
          Data = exception.StackTrace
        };

      var result = JsonConvert.SerializeObject(response);
      context.Response.ContentType = "application/json";
      return context.Response.WriteAsync(result);
    }
  }
}
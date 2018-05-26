﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommonLibraries.Exceptions
{
  public class ExceptionHandlingMiddleware
  {

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
    //  _logger = logger;
    }

    public async Task Invoke(HttpContext context, IHostingEnvironment env, ILoggerFactory loggerFactory /* other dependencies */)
    {
      try
      {
        await _next.Invoke(context);
      }
      catch (Exception ex)
      {
       // _logger.LogError(EventIds.GlobalException, ex, ex.Message);
        await HandleExceptionAsync(context, env, loggerFactory, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, IHostingEnvironment env, ILoggerFactory loggerFactory, Exception exception)
    {
      var response = new ResponseObject();
      var error = exception.Message + "\n"+exception.StackTrace;

      if (!env.IsDevelopment())
      {
        error = "INTERNAL SERVER ERROR";
      }
      response.Message = error;

      var result = JsonConvert.SerializeObject(response);
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = 500;
      return context.Response.WriteAsync(result);
    }
  }
}

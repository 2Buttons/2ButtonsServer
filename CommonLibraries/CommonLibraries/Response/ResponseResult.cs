using System;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace CommonLibraries.Response
{
  public class ResponseResult : IActionResult
  {
    protected readonly ResponseObject Response;

    public ResponseResult(int status, string message = null, object data = null)
    {
      Response = new ResponseObject(status, message ?? GetDafaultMesage(status), data);
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
      context.HttpContext.Response.ContentType = "application/json";
      context.HttpContext.Response.StatusCode = Response.Status;
      await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(Response));
    }

    private static string GetDafaultMesage(int statusCode)
    {
      switch (statusCode)
      {
        case 200: return "OK";
        case 201: return "CREATED";
        case 304: return "NOT MODIFIED";
        case 400: return "BAD REQUEST";
        case 401: return "UNAUTHORIZED";
        case 403: return "FORBIDDEN";
        case 404: return "NOT FOUND";
        case 409: return "CONFLICT";
        case 500: return "INTERNAL SERVER ERROR";
        default: return null;
      }
    }
  }

  public class OkResponseResult : ResponseResult
  {
    public OkResponseResult() : this(null, null) { }
    public OkResponseResult(string message) : this(message, null) { }
    public OkResponseResult(object data) : this(null, data) { }
    public OkResponseResult(string message, object data) : base(200, message, data) { }
  }

  public class BadResponseResult : ResponseResult
  {
    public BadResponseResult() : this(null, null) { }
    public BadResponseResult(string message) : this(message, null) { }
    public BadResponseResult(object data) : this(null, data) { }

    public BadResponseResult(ModelStateDictionary modelState) : base(400)
    {
      if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));

      var errors = modelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToArray();
      Response.Message = "Validation errors";
      Response.Data = errors;
    }

    public BadResponseResult(string message, object data) : base(400, message, data)
    {
      if (message.IsNullOrEmpty() && data == null) Response.Message = "Input body is null.";
    }
  }
}
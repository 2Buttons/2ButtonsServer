using System;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CommonLibraries.Response
{
  public class ResponseResult : IActionResult
  {
    protected readonly ResponseObject Response;

    public ResponseResult(int status) : this(status, null, null) { }
    public ResponseResult(int status, string message) : this(status, message, null) { }
    public ResponseResult(int status, object data) : this(status, null, data) { }
    public ResponseResult(int status, string message, object data)
    {
      Response = new ResponseObject(status, message, data);
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
      var serializerSettings =
        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
      context.HttpContext.Response.ContentType = "application/json";
      context.HttpContext.Response.StatusCode = Response.Status;
      await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(Response, serializerSettings));
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

      var errors = modelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToList();
      Response.Message = "Validation errors";
      Response.Data = new {Errors = errors, Keys = modelState.Keys, Values = modelState.Values};
    }

    public BadResponseResult(string message, object data) : base(400, message, data)
    {
      if (message.IsNullOrEmpty() && data == null) Response.Message = "Input body is null.";
    }
  }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommonLibraries.ApiResponse
{
  public class ResponseResult : IActionResult
  {
    private readonly ResponseObject _response;

    public ResponseResult(object data, int status, string message)
    {
      _response = new ResponseObject(status, message, data);
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
      context.HttpContext.Response.StatusCode = _response.Status;
      await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(_response));
    }
  }

  public class OkResponseResult : ResponseResult
  {
    public OkResponseResult() : this("Ok") { }
    public OkResponseResult(string message) : this(null, message) { }
    public OkResponseResult(object data) : this("Ok",data) { }
    public OkResponseResult(string message, object data) : base(data, 200, message) { }
  }

  public class BadResponseResult : ResponseResult
  {
    public BadResponseResult() : this("Bad request") { }
    public BadResponseResult(string message) : this(null, message) { }
    public BadResponseResult(object data) : this("Bad request",data) { }
    public BadResponseResult(string  message, object data) : base(data, 400, message) { }
  }


  //public class ResponseResult<T> : IActionResult
  //{
  //  private readonly ResponseObject<T> _response;

  //  public ResponseResult(T data, int status, string message)
  //  {
  //    _response = new ResponseObject<T>(status, message, data);
  //  }

  //  public async Task ExecuteResultAsync(ActionContext context)
  //  {
  //    context.HttpContext.Response.StatusCode = _response.Status;
  //    await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(_response));
  //  }
  //}
}
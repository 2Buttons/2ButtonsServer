using Newtonsoft.Json;

namespace CommonLibraries.Response
{
  public class ResponseObject
  {
    [JsonProperty("status")]
    public int Status { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    public object Data { get; set; }

    public ResponseObject()
    {
    }

    public ResponseObject(int status, string message, object data)
    {
      Status = status;
      Message = string.IsNullOrEmpty(message) ? GetDafaultMesage(status) : message;
      Data = data;
    }

    public static string GetDafaultMesage(int statusCode)
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
        default: return "DEFAULT INTERNAL SERVER ERROR";
      }
    }
  }

  public class ResponseObject<T>
  {
    [JsonProperty("status")]
    public int Status { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    public T Data { get; set; }

    public ResponseObject()
    {
    }

    public ResponseObject(int status, string message, T data)
    {
      Status = status;
      Message = message ?? GetDafaultMesage(status);
      Data = data;
    }

    public static string GetDafaultMesage(int statusCode)
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
        default: return "DEFAULT INTERNAL SERVER ERROR";
      }
    }
  }
}
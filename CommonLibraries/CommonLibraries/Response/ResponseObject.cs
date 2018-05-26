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
      Message = message;
      Data = data;
    }
  }
}
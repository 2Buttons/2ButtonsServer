namespace CommonLibraries.Response
{
  public class ResponseObject
  {
    public int Status { get; set; }
    public string Message { get; set; }
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

  public class ResponseObject<T>
  {
    public int Status { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public ResponseObject()
    {
    }

    public ResponseObject(int status, string message, T data)
    {
      Status = status;
      Message = message;
      Data = data;
    }
  }
}
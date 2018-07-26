namespace DataGenerator.Services
{
  public class SizedUrl<T> where T : struct
  {
    public T Size { get; set; }
    public string Url { get; set; }
  }
}

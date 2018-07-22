using CommonLibraries;

namespace MediaServer.ViewModel
{
  public class GetDefaultsViewModel
  {
    public DefaultSizeType DefaultSizeType { get; set; } = DefaultSizeType.Original;
    public string Pattern { get; set; }
  }
}
using CommonLibraries;

namespace MediaServer.ViewModel
{
  public class GetDefaultsViewModel
  {
    public DefaultSizeType SizeType { get; set; } = DefaultSizeType.Original;
    public string Pattern { get; set; }
  }
}
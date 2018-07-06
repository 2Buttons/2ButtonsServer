namespace BotsServer.ViewModels.Input
{
    public class GetGuestionsByPatternViewModel : ClientIdentity
  {
    public string Pattern { get; set; }
    public PageParams PageParams { get; set; }
    }
}

namespace BotsServer.ViewModels.Input
{
  public class GetQuestionsViewModel : ClientIdentity
  {
    public PageParams PageParams { get; set; } = new PageParams();
  }
}
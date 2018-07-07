namespace BotsServer.ViewModels.Input
{
  public class GetQuestions : ClientIdentity
  {
    public PageParams PageParams { get; set; } = new PageParams();
  }
}
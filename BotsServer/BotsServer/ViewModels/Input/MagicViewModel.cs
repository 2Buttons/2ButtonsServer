namespace BotsServer.ViewModels.Input
{
  public class MagicViewModel : ClientIdentity
  {
    public int QuestionId { get; set; }
    public int FirstOptionPercent { get; set; }
    public int SecondOptionPercent { get; set; }
  }
}
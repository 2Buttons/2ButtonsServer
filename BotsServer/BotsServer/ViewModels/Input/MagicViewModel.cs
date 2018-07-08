namespace BotsServer.ViewModels.Input
{
  public class MagicViewModel : ClientIdentity
  {
    public int QuestionId { get; set; }
    public int FirstOptionPercent { get; set; }
    public int SecondOptionPercent { get; set; }
    public int Interval { get; set; }
    public int BotsCount { get; set; }
    public int BotsPerVote { get; set; }
  }
}
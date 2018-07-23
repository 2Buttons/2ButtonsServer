using CommonLibraries;

namespace QuestionsServer.ViewModels.OutputParameters.NewsQuestions
{
  public class NewsUserViewModel
  {
    public long UserId { get; set; }
    public string Login { get; set; }
    public SexType SexType { get; set; }
  }
}
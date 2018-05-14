using Newtonsoft.Json;

namespace TwoButtonsServer.ViewModels.OutputParameters.NewsQuestions
{
  public class NewsQuestionBaseViewModel : QuestionBaseViewModel
  {
    public int AnsweredFollowToAmount { get; set; }
    public int Position { get; set; }

    [JsonIgnore]
    public int Priority { get; set; }
  }
}

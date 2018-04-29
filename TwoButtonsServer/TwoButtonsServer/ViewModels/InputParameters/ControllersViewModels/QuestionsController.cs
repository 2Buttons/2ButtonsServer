using System.Collections.Generic;
using TwoButtonsServer.ViewModels.OutputParameters;

namespace TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels
{
  public class AddQuestionViewModel : UserIdViewModel
  {
    public string Condition { get; set; }
    public int Anonymity { get; set; }
    public int Audience { get; set; }
    public int QuestionType { get; set; }
    public string FirstOption { get; set; }
    public string SecondOption { get; set; }
    public string BackgroundImageLink { get; set; } = null;

    public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
  }

  public class AddAnswerViewModel : QuestionIdViewModel
  {
    public AnswerType AnswerType { get; set; }
    public FeedbackType YourFeedbackType { get; set; }
  }

  public class AddComplaintViewModel : QuestionIdViewModel
  {
    public int ComplaintId { get; set; }
  }

  public class AddRecommendedQuestionViewModel
  {
    public int UserToId { get; set; }
    public int UserFromId { get; set; }
    public int QuestionId { get; set; }
  }


  public class GetVoters : QuestionViewModel
  {
    public AnswerType AnswerType { get; set; }
    public int MinAge { get; set; } = 0;
    public int MaxAge { get; set; } = 100;
    public SexType SexType { get; set; } = 0;
    public string SearchedLogin { get; set; } = "";
  }
}
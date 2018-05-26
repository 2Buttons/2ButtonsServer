using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;

namespace QuestionsServer.ViewModels.InputParameters.ControllersViewModels
{
  public class AddQuestionViewModel : UserIdViewModel
  {
    [Required]
    public string Condition { get; set; }
    public bool IsAnonymity { get; set; }
    public bool IsAudience { get; set; }
    public QuestionType QuestionType { get; set; }
    [Required]
    public string FirstOption { get; set; }
    [Required]
    public string SecondOption { get; set; }
    public string BackgroundImageLink { get; set; } = null;

    public List<string> Tags { get; set; } = new List<string>();
  }

  public class UpdateQuestionFeedbackViewModel : QuestionIdViewModel
  {
    public FeedbackType FeedbackType { get; set; }
  }

  public class UpdateQuestionAnswerViewModel : QuestionIdViewModel
  {
    public AnswerType AnswerType { get; set; }
  }

  public class UpdateQuestionFavoriteViewModel : QuestionIdViewModel
  {
    public bool IsInFavorites { get; set; }
  }


  public class AddAnswerViewModel : QuestionIdViewModel
  {
    public AnswerType AnswerType { get; set; }
    public FeedbackType YourFeedbackType { get; set; }
  }

  public class AddComplaintViewModel : QuestionIdViewModel
  {
    public ComplainType ComplainType { get; set; }
  }

  public class AddRecommendedQuestionViewModel
  {
    [IdValidationt]
    public int UserToId { get; set; }
    [IdValidationt]
    public int UserFromId { get; set; }
    [IdValidationt]
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
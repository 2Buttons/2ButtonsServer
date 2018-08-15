using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;
using Microsoft.AspNetCore.Http;

namespace QuestionsServer.ViewModels.InputParameters.ControllersViewModels
{
  public class AddQuestionViewModel : UserIdViewModel
  {
    [Required]
    public string Condition { get; set; }
    public AudienceType AudienceType { get; set; }
    public QuestionType QuestionType { get; set; }
    [Required]
    public string FirstOption { get; set; }
    [Required]
    public string SecondOption { get; set; }
    public string BackgroundUrl { get; set; } = null;

    public List<string> Tags { get; set; } = new List<string>();

    public List<int> RecommendedToIds { get; set; } = new List<int>();
  }

  public class GetQuestionBackground 
  {
    [IdValidation(nameof(QuestionId))]
    public int QuestionId { get; set; }
    public BackgroundSizeType BackgroundSizeType { get; set; } = BackgroundSizeType.Original;
  }


  public class GetStandardBackgroundUrlsViewModel
  {
    public BackgroundSizeType BackgroundSizeType { get; set; } = BackgroundSizeType.Original;
  }

  public class GetCustomBackgroundUrlsViewModel : UserIdViewModel
  {
    public BackgroundSizeType BackgroundSizeType { get; set; } = BackgroundSizeType.Original;
  }

  public class GetQuestionFilteredStatistics : QuestionIdViewModel
  {
    public int MinAge { get; set; } = 0;
    public int MaxAge { get; set; } = 100;
    public SexType SexType { get; set; } = SexType.Both;
    public string City { get; set; } = null;
    public PageParams PageParams { get; set; } = new PageParams();
  }

  public class UpdateQuestionFeedbackViewModel : QuestionIdViewModel
  {
    public QuestionFeedbackType FeedbackType { get; set; }
  }

  public class UpdateQuestionAnswerViewModel : QuestionIdViewModel
  {
    public AnswerType AnswerType { get; set; }
  }

  public class GetQuestionByCommentId: UserIdViewModel
  {
    [Required]
    [IdValidation(nameof(CommentId))]
    public int CommentId { get; set; }

    public BackgroundSizeType BackgroundSizeType { get; set; } = BackgroundSizeType.Original;
  }

  public class UploadQuestionBackgroundViaUrlViewModel : QuestionIdViewModel
  {
    public BackgroundSizeType BackgroundSizeType { get; set; } = BackgroundSizeType.Original;
    [Required]
    public string Url { get; set; }
  }

  public class UploadQuestionBackgroundViaFileViewModel : QuestionIdViewModel
  {
    public BackgroundSizeType BackgroundSizeType { get; set; } = BackgroundSizeType.Original;
    [Required]
    public IFormFile File { get; set; }
  }

  public class GetQuestion : QuestionIdViewModel
  {
    public BackgroundSizeType BackgroundSizeType { get; set; } = BackgroundSizeType.Original;
  }



  public class UpdateQuestionFavoriteViewModel : QuestionIdViewModel
  {
    public bool IsFavorite { get; set; }
  }


  public class AddAnswerViewModel : QuestionIdViewModel
  {
    public AnswerType AnswerType { get; set; }
    public FeedbackType YourFeedbackType { get; set; }
  }

  public class AddComplaintViewModel : QuestionIdViewModel
  {
    public ComplaintType ComplaintType { get; set; }
  }

  public class AddRecommendedQuestionViewModel
  {
    [Required]
    public List<int> UsersToId { get; set; }
    [IdValidation]
    public int UserFromId { get; set; }
    [IdValidation]
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
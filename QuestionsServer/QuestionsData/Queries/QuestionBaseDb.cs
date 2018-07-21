using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace QuestionsData.Queries
{
  public class QuestionBaseDb
  {
    [Key]
    public int QuestionId { get; set; }

    public string Condition { get; set; }
    public string FirstOption { get; set; }
    public string SecondOption { get; set; }
    public string OriginalBackgroundUrl { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime AddedDate { get; set; }
    public int UserId { get; set; }
    public string Login { get; set; }
    public SexType SexType { get; set; }
    public string OriginalAvatarUrl { get; set; }
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }
    public FeedbackType YourFeedbackType { get; set; }
    public AnswerType YourAnswerType { get; set; }
    public bool InFavorites { get; set; }
    public bool IsSaved { get; set; }
    public int CommentsCount { get; set; }

    public int FirstAnswersCount { get; set; }
    public int SecondAnswersCount { get; set; }
  }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public string BackgroundImageUrl { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime QuestionAddDate { get; set; }
    public int UserId { get; set; }
    public string Login { get; set; }
    [Column("sex")]
    public SexType SexType { get; set; }
    public string OriginalAvatarUrl { get; set; }
    [Column("likes")]
    public int LikesCount { get; set; }
    [Column("dislikes")]
    public int DislikesCount { get; set; }
    public FeedbackType YourFeedback { get; set; }
    public AnswerType YourAnswer { get; set; }
    public bool InFavorites { get; set; }
    public bool IsSaved { get; set; }
    [Column("comments")]
    public int CommentsCount { get; set; }

    [Column("firstAnswers")]
    public int FirstAnswersCount { get; set; }
    [Column("secondAnswers")]
    public int SecondAnswersCount { get; set; }
  }
}
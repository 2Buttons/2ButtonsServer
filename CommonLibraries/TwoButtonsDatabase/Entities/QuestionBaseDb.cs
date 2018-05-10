using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace TwoButtonsDatabase.Entities
{
  public class QuestionBaseDb
  {
    [Key]
    public int QuestionId { get; set; }

    public string Condition { get; set; }
    public string FirstOption { get; set; }
    public string SecondOption { get; set; }
    public string BackgroundImageLink { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime QuestionAddDate { get; set; }
    public int UserId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarLink { get; set; }
    public int Shows { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public FeedbackType YourFeedback { get; set; }
    public AnswerType YourAnswer { get; set; }
    public int InFavorites { get; set; }
    public bool IsSaved { get; set; }
    public int Comments { get; set; }

    public int FirstAnswers { get; set; }
    public int SecondAnswers { get; set; }
  }
}
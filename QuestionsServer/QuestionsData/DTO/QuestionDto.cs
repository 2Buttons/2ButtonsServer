using System;
using System.Collections.Generic;
using CommonLibraries;

namespace QuestionsData.DTO
{
  public class QuestionDto
  {
    public int QuestionId { get; set; }
    public string Condition { get; set; }
    public List<OptionDto> Options { get; set; }
    public string OriginalBackgroundUrl { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime QuestionAddDate { get; set; }
    public int UserId { get; set; }
    public string Login { get; set; }
    public SexType SexType { get; set; }
    public string SmallAvatarUrl { get; set; }
    public int LikesAmount { get; set; }
    public int DislikesAmount { get; set; }
    public FeedbackType YourFeedbackType { get; set; }
    public AnswerType YourAnswerType { get; set; }
    public bool IsInFavorites { get; set; }
    public bool IsSaved { get; set; }
    public int CommentsAmount { get; set; }
  }
}
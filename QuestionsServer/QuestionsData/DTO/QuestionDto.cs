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
    public string OriginalAvatarUrl { get; set; }
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }
    public QuestionFeedbackType YourFeedbackType { get; set; }
    public AnswerType YourAnswerType { get; set; }
    public bool IsInFavorites { get; set; }
    public bool IsSaved { get; set; }
    public int CommentsCount { get; set; }
  }
}
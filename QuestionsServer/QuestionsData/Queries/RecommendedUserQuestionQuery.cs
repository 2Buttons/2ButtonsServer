using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Queries
{
  public class RecommendedUserQuestionQuery
  {
    [Key]
    public int QuestionId { get; set; }

    public int UserId { get; set; }
  }
}
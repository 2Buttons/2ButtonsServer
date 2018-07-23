using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Queries
{
  public class RecommendedUserQuestionQuery
  {
    [Key]
    public long QuestionId { get; set; }

    public long UserId { get; set; }
  }
}
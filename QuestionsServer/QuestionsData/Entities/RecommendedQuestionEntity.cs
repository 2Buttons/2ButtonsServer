using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Entities
{
  public class RecommendedQuestionEntity
  {
    [Key]
    public int QuestionId { get; set; }

    public int UserToId { get; set; }
    public int UserFromId { get; set; }
  }
}
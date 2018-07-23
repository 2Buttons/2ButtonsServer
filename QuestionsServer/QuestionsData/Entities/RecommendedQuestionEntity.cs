using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsData.Entities
{
  [Table("RecommendedQuestions")]
  public class RecommendedQuestionEntity
  {
    [Key]
    public long QuestionId { get; set; }

    public int UserToId { get; set; }
    public int UserFromId { get; set; }
  }
}
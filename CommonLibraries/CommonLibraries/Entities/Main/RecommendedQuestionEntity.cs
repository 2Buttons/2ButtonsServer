using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Entities.Main
{
  [Table("RecommendedQuestions")]
  public class RecommendedQuestionEntity
  {
    [Key]
    public int QuestionId { get; set; }

    public int UserToId { get; set; }
    public int UserFromId { get; set; }
  }
}
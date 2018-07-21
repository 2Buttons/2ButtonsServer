using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Queries
{
  public class TagDb
  {
    [Key]
    public int TagId { get; set; }

    public string Text { get; set; }
    public int Position { get; set; }
  }
}
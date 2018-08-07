using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuestionsData.Entities
{
  [Table("Favorites")]
  public class FavoriteEntity
  {
    [Key]
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    public bool IsAnonymous { get; set; }
    public bool? IsDeleted { get; set; }
  }
}

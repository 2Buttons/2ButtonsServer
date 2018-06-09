using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsData.Entities
{
  [Table("Follow")]
  public class FollowEntity
  {
    [Column("FollowerId")]
    [Key]
    public int UserdId { get; set; }

    public int FollowToId { get; set; }
  }
}
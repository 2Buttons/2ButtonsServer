using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsData.Entities
{
  [Table("Followers")]
  public class FollowEntity
  {
    [Key]
    public int FollowerId { get; set; }

    public int FollowingId { get; set; }
  }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace QuestionsData.Entities
{
  [Table("User")]
  public class UserEntity
  {
    [Key]
    public int UserId { get; set; }

    public string Login { get; set; }
    public DateTime BirthDate { get; set; }

   [Column("sex")]
    public SexType SexType { get; set; }

    public int CityId { get; set; }
    public string Description { get; set; }
    public DateTime LastNotsSeenDate { get; set; }
    public string FullAvatarLink { get; set; }
    public string SmallAvatarLink { get; set; }
  }
}
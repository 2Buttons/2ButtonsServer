using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Entities.Main
{
  [Table("Users")]
  public class UserInfoEntity
  {
    [Key]
    public int UserId { get; set; }

    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public int CityId { get; set; }
    public string Description { get; set; }
    public DateTime? LastNotsSeenDate { get; set; }
    public string OriginalAvatarUrl { get; set; }
  }
}
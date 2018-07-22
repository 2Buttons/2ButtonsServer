using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationsData.Main.Entities
{
  [Table("Users")]
  public class UserInfoDb
  {
    [Key]
    public int UserId { get; set; }

    public string Login { get; set; }
    public string OriginalAvatarUrl{ get; set; }

    public DateTime? LastNotsSeenDate { get; set; }
  }
}
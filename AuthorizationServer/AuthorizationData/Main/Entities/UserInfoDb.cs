using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace AuthorizationData.Main.Entities
{
  public class UserInfoDb
  {
    [Key]
    public int UserId { get; set; }

    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    [Column("Sex")]
    public SexType SexType { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public string OriginaltAvatarUrl { get; set; }
  }
}
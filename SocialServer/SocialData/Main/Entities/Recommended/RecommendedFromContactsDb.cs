using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace SocialData.Main.Entities.Recommended
{
  public class RecommendedFromContactsDb
  {
    [Key]
    public int UserId { get; set; }

    public string Login { get; set; }
    public string AvatarLink { get; set; }
    public DateTime BirthDate { get; set; }

    [Column("Sex")]
    public SexType SexType { get; set; }

    public SocialType NetworkId { get; set; }
  }
}
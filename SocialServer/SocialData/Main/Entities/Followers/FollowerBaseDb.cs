using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace SocialData.Main.Entities.Followers
{
  public class FollowerBaseDb
  {
    [Key]
    public int UserId { get; set; }

    public string Login { get; set; }
    public string OriginalAvatarLink { get; set; }
    public DateTime BirthDate { get; set; }
    [Column("Sex")]
    public SexType SexType { get; set; }
    public bool YouFollowed { get; set; }
    public bool HeFollowed { get; set; }
  }
}
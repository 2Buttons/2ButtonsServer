using System.ComponentModel.DataAnnotations;

namespace SocialData.Main.Entities
{
  public class NewFollowersDb
  {
    [Key]
    public int FollowerId { get; set; }

    public bool Followed { get; set; }
    public string Login { get; set; }
    public string SmallAvatarUrl { get; set; }
  }
}
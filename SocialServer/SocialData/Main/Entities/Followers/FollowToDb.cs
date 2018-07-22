using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace SocialData.Main.Entities.Followers
{
  public partial class FollowToDb : FollowerBaseDb
  {
    public int VisitsCount{ get; set; }
  }
}

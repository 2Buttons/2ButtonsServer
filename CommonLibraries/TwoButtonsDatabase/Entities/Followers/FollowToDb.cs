using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace TwoButtonsDatabase.Entities.Followers
{
  public partial class FollowToDb : FollowerBaseDb
  {
    public int Visits{ get; set; }
  }
}

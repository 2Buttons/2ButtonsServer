using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace TwoButtonsDatabase.Entities.Followers
{
  public partial class FollowToDb :FollowerDb
  {
    public int Visits{ get; set; }
  }
}

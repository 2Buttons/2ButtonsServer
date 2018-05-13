using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
  public class RecommendedFromUsersIdDb
  {
    [Key]
    public int UserId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarLink { get; set; }
  }
}
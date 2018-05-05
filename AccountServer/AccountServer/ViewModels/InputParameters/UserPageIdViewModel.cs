using System.ComponentModel.DataAnnotations;

namespace AccountServer.ViewModels.InputParameters
{
  public class UserPageIdViewModel
  {
    [Required]
    public int UserId { get; set; }
    [Required]
    public int UserPageId { get; set; }
  }
}
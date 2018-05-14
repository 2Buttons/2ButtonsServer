using System.ComponentModel.DataAnnotations;

namespace AccountServer.ViewModels.InputParameters
{
  public class UserPageIdViewModel : UserIdViewModel
  {
    [Required]
    public int UserPageId { get; set; }
  }
}
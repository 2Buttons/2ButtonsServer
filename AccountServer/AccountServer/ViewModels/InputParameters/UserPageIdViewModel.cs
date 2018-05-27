using System.ComponentModel.DataAnnotations;
using CommonLibraries.Validation;

namespace AccountServer.ViewModels.InputParameters
{
  public class UserPageIdViewModel : UserIdViewModel
  {
    [Required]
    [IdValidation(nameof(UserPageId))]
    public int UserPageId { get; set; }
  }
}
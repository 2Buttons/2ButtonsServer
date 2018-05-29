using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;
using Microsoft.AspNetCore.Http;

namespace AccountServer.ViewModels.InputParameters
{
  public class UpdateAvatarUrlViewModel
  {
    [Required]
    [IdValidation(nameof(UserId))]
    public int UserId { get; set; }

    [Required]
    public AvatarSizeType Size { get; set; }

    [Required]
    public string Url { get; set; }
  }

  public class UpdateAvatarFileViewModel
  {
    [Required]
    [IdValidation(nameof(UserId))]
    public int UserId { get; set; }

    [Required]
    public AvatarSizeType Size { get; set; }

    [Required]
    public IFormFile File { get; set; }
  }
}
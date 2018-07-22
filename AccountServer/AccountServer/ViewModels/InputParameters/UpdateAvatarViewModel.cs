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

    public AvatarType AvatarType { get; set; } = AvatarType.Custom;

    public AvatarSizeType AvatarSizeType { get; set; } = AvatarSizeType.Original;

    [Required]
    public string Url { get; set; }
  }

  public class UpdateAvatarFileViewModel
  {
    [Required]
    [IdValidation(nameof(UserId))]
    public int UserId { get; set; }

    public AvatarType AvatarType { get; set; } = AvatarType.Custom;

    public AvatarSizeType AvatarSizeType { get; set; } = AvatarSizeType.Original;

    [Required]
    public IFormFile File { get; set; }
  }
}
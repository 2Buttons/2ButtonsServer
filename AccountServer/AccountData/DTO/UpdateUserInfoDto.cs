using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;

namespace AccountData.DTO
{
  public class UpdateUserInfoDto
  {
    [Required]
    [IdValidation]
    public int UserId { get; set; }
    [Required]
    public string Login { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    [Required]
    public SexType Sex { get; set; }
    [Required]
    public string City { get; set; }
    public string Description { get; set; }
    public string LargeAvatarLink { get; set; }
    public string SmallAvatarLink { get; set; }
  }
}
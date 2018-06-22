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
    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType Sex { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public string LargeAvatarLink { get; set; }
    public string SmallAvatarLink { get; set; }
  }
}
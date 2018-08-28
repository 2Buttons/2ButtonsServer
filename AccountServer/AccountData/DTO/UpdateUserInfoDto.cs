using System;
using CommonLibraries;

namespace AccountData.DTO
{
  public class UpdateUserInfoDto
  {
    public int UserId { get; set; }
    public string FristName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public string City{ get; set; }
    public string Description { get; set; }
  }
}
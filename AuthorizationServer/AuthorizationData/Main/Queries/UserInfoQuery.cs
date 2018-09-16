using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace AuthorizationData.Main.Entities
{
  public class UserInfoQuery
  {
    public int UserId { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public string OriginalAvatarUrl { get; set; }
  }
}
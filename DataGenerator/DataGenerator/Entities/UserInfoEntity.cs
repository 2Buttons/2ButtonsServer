using System;
using CommonLibraries;

namespace DataGenerator.Entities
{
  public class UserInfoEntity
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public int CityId { get; set; }
    public string Description { get; set; }
    public DateTime LastNotsSeenDate { get; set; }
    public string LargeAvatarLink { get; set; }
    public string SmallAvatarLink { get; set; }
  }
}
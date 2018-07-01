using System;
using DataGenerator.ReaderObjects;

namespace CommonLibraries.SocialNetworks
{
  public class UserDataFromVk
  {
    public long ExternalId { get; set; }
    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public string LargePhotoUrl { get; set; }
    public string SmallPhotoUrl { get; set; }
  }
}
using System;

namespace CommonLibraries.SocialNetworks
{
  public class NormalizedSocialUserData
  {
    public long ExternalId { get; set; }
    public string ExternalEmail { get; set; }
    public string ExternalToken { get; set; }
    public int ExpiresIn { get; set; }
    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType Sex { get; set; }
    public string City { get; set; }
    public string LargePhotoUrl { get; set; }
    public string SmallPhotoUrl { get; set; }
  }
}
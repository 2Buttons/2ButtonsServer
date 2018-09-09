using System;

namespace CommonLibraries.SocialNetworks
{
  public class NormalizedSocialUserData
  {
    public long ExternalId { get; set; }
    public string ExternalEmail { get; set; }
    public string ExternalToken { get; set; }
    public long ExpiresIn { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public string City { get; set; }
    public string OriginalPhotoUrl { get; set; }
  }
}
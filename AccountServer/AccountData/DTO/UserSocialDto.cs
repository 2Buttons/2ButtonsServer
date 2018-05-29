using CommonLibraries;

namespace AccountData.DTO
{
  public class UserSocialDto
  {
    public SocialType SocialType { get; set; }
    public long ExternalId { get; set; }
  }
}

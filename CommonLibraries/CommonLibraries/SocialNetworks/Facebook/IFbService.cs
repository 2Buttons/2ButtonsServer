using System.Threading.Tasks;

namespace CommonLibraries.SocialNetworks.Facebook
{
  public interface IFbService
  {
    Task<FacebookAppAccessToken> GetAccessTokenAsync(string fbCode);
    Task<NormalizeSocialUserData> GetUserInfoAsync(string code);
  }
}
using System.Threading.Tasks;
using SocialServer.ViewModels.InputParameters;
using SocialServer.ViewModels.OutputParameters;

namespace SocialServer.Infrastructure
{
  public interface IFriendsService
  {
    Task<RecommendedUsers> GetRecommendedUsers(GetRecommendedUsers user);
  }
}
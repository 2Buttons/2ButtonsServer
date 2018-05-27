using System.Threading.Tasks;
using AccountData.DTO;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;

namespace AccountServer.Infrastructure.Services
{
  public interface IAccountService
  {
    Task<UserInfoViewModel> GetUserAsync(int userId, int userPageId);
    Task<bool> UpdateUserInfoAsync(UpdateUserInfoDto user);
    Task<bool> AddUserSocialAsync(int userId, string code, SocialType socialType);
  }
}
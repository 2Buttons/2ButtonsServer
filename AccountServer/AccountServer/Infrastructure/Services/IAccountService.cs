using System.Threading.Tasks;
using AccountServer.ViewModels.OutputParameters.User;

namespace AccountServer.Infrastructure.Services
{
  public interface IAccountService
  {
    Task<UserInfoViewModel> GetUserAsync(int userId, int userPageId);
  }
}
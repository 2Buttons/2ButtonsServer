using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.Entities.Main;

namespace AccountServer.Infrastructure.Services
{
  public interface IFeedbackService
  {
    Task<bool> AddFeedback(FeedbackEntity feedback);
    Task<List<FeedbackEntity>> GetFeedbacks(int offset, int count);
    Task<List<FeedbackEntity>> GetUserFeedbacks(int userId);
  }
}
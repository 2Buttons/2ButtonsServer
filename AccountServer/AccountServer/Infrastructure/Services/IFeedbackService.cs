using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData.Main.Entities;

namespace AccountServer.Infrastructure.Services
{
  public interface IFeedbackService
  {
    Task<bool> AddFeedback(FeedbackDb feedback);
    Task<List<FeedbackDb>> GetFeedbacks(int offset, int count);
    Task<List<FeedbackDb>> GetUserFeedbacks(int userId);
  }
}
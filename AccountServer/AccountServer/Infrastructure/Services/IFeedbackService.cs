using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData.Main.Entities;

namespace AccountServer.Infrastructure.Services
{
  public interface IFeedbackService
  {
    Task<List<FeedbackDb>> GetUserFeedbacks(int userId);
    Task<bool> AddFeedback(FeedbackDb feedback);
  }
}
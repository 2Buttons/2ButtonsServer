using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData;
using AccountData.Main.Entities;

namespace AccountServer.Infrastructure.Services
{
  public class FeedbackService : IFeedbackService
  {
    private readonly AccountDataUnitOfWork _db;

    public FeedbackService(AccountDataUnitOfWork accountDb)
    {
      _db = accountDb;
    }

    public async Task<bool> AddFeedback(FeedbackEntity feedback)
    {
      return await _db.Feedbacks.AddFeedbackAsync(feedback);
    }

    public async Task<List<FeedbackEntity>> GetUserFeedbacks(int userId)
    {
      return await _db.Feedbacks.GetFeedbacksByUserAsync(userId);
    }

    public async Task<List<FeedbackEntity>> GetFeedbacks(int offset, int count)
    {
      return await _db.Feedbacks.GetFeedbacks(offset, count);
    }
  }
}
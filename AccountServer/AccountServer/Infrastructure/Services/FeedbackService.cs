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

    public async Task<bool> AddFeedback(FeedbackDb feedback)
    {
      return await _db.Feedbacks.AddFeedbackAsync(feedback);
    }

    public async Task<List<FeedbackDb>> GetUserFeedbacks(int userId)
    {
      return await _db.Feedbacks.GetFeedbacksByUserAsync(userId);
    }
  }
}
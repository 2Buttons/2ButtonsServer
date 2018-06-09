using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountData.Main.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountData.Main.Repositories
{
  public class FeedbackRepository
  {
    private readonly TwoButtonsContext _db;

    public FeedbackRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<bool> AddFeedbackAsync(FeedbackEntity feedback)
    {
      _db.FeedbackEntities.Add(feedback);
      return await _db.SaveChangesAsync() > 0;
    }

    public async Task<List<FeedbackEntity>> GetFeedbacksByUserAsync(int userId)
    {
      return await _db.FeedbackEntities.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<FeedbackEntity> FindUserFeedbackAsync(int feedbackId)
    {
      return await _db.FeedbackEntities.FindAsync(feedbackId);
    }

    public async Task<List<FeedbackEntity>> GetFeedbacks(int offset, int count)
    {
      return await _db.FeedbackEntities.Skip(offset).Take(count).ToListAsync();
    }
  }
}
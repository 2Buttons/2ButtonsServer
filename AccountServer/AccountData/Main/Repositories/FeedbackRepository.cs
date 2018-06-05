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

    public async Task<bool> AddFeedbackAsync(FeedbackDb feedback)
    {
      _db.FeedbacksDb.Add(feedback);
      return await _db.SaveChangesAsync() > 0;
    }

    public async Task<List<FeedbackDb>> GetFeedbacksByUserAsync(int userId)
    {
      return await _db.FeedbacksDb.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<FeedbackDb> FindUserFeedbackAsync(int feedbackId)
    {
      return await _db.FeedbacksDb.FindAsync(feedbackId);
    }

    public async Task<List<FeedbackDb>> GetFeedbacks(int offset, int count)
    {
      return await _db.FeedbacksDb.Skip(offset).Take(count).ToListAsync();
    }
  }
}
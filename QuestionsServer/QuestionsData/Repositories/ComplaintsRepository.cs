using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuestionsData.Entities.Moderators;

namespace QuestionsData.Repositories
{
  public class ComplaintsRepository
  {
    private readonly TwoButtonsContext _db;

    public ComplaintsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<bool> AddComplaint(int userId, int questionId, int complaintId)
    {
      var complaintAddDate = DateTime.UtcNow;
      try
      {
        return  await _db.Database.ExecuteSqlCommandAsync($"addComplaint {userId}, {questionId}, {complaintId}, {complaintAddDate}") >0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public async Task<List<ComplaintDb>> GetComplaints()
    {
      try
      {
        return await _db.ComplaintDb.AsNoTracking().FromSql($"select * from dbo.getComplaints()").ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<ComplaintDb>();
    }
  }
}
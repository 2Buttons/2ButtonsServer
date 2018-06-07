using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using QuestionsData.Queries.Moderators;

namespace QuestionsData.Repositories
{
  public class ComplaintsRepository
  {
    private readonly TwoButtonsContext _db;

    public ComplaintsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<bool> AddComplaint(int userId, int questionId, ComplaintType complaintType)
    {
      var complainttAddDate = DateTime.UtcNow;

      return await _db.Database.ExecuteSqlCommandAsync(
               $"addComplaint {userId}, {questionId}, {complaintType}, {complainttAddDate}") > 0;
    }

    public async Task<List<ComplaintDb>> GetComplaints()
    {
      return await _db.ComplaintDb.AsNoTracking().FromSql($"select * from dbo.getComplaints()").ToListAsync() ??
             new List<ComplaintDb>();
    }
  }
}
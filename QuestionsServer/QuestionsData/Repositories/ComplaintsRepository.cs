using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using QuestionsData.Entities.Moderators;

namespace QuestionsData.Repositories
{
  public class ComplainttsRepository
  {
    private readonly TwoButtonsContext _db;

    public ComplainttsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<bool> AddComplaintt(int userId, int questionId, ComplaintType complaintType)
    {
      var complainttAddDate = DateTime.UtcNow;

      return await _db.Database.ExecuteSqlCommandAsync(
               $"addComplaintt {userId}, {questionId}, {complaintType}, {complainttAddDate}") > 0;
    }

    public async Task<List<ComplaintDb>> GetComplaintts()
    {
      return await _db.ComplainttDb.AsNoTracking().FromSql($"select * from dbo.getComplaintts()").ToListAsync() ??
             new List<ComplaintDb>();
    }
  }
}
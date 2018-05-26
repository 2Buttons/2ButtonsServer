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
      try
      {
        return  await _db.Database.ExecuteSqlCommandAsync($"addComplaintt {userId}, {questionId}, {complaintType}, {complainttAddDate}") >0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public async Task<List<ComplaintDb>> GetComplaintts()
    {
      try
      {
        return await _db.ComplainttDb.AsNoTracking().FromSql($"select * from dbo.getComplaintts()").ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<ComplaintDb>();
    }
  }
}
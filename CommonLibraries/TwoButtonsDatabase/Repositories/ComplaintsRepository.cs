using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities.Moderators;

namespace TwoButtonsDatabase.Repositories
{
  public class ComplaintsRepository
  {
    private readonly TwoButtonsContext _db;

    public ComplaintsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public  bool TryAddComplaint( int userId, int questionId, int complaintId)
    {
      var complaintAddDate = DateTime.UtcNow;
      try
      {
        _db.Database.ExecuteSqlCommand($"addComplaint {userId}, {questionId}, {complaintId}, {complaintAddDate}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public  bool TryGetComplaints( out IEnumerable<ComplaintDb> complaints)
    {
      try
      {
        complaints = _db.ComplaintDb.FromSql($"select * from dbo.getComplaints()").ToList();
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      complaints = new List<ComplaintDb>();
      return false;
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.Moderators;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public class ModeratorWrapper
    {
        public static bool TryAddComplaint(TwoButtonsContext db, int userId, int questionId, int complaintId)
        {
            var complaintAddDate = DateTime.UtcNow;
            try
            {
                db.Database.ExecuteSqlCommand($"addComplaint {userId}, {questionId}, {complaintId}, {complaintAddDate}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public static bool TryGetComplaints(TwoButtonsContext db, out IEnumerable<ComplaintDb> complaints)
        {

            try
            {
                complaints = db.ComplaintDb.FromSql($"select * from dbo.getComplaints()").ToList();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            complaints = new List<ComplaintDb>();
            return false;

        }

        public static bool TryGetUserRole(TwoButtonsContext db, int userId, out int role)
        {
            try
            {
                role = db.RoleDb.FromSql($"select * from dbo.identification({userId})").FirstOrDefault()?.RoleId ?? 1;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            role = 1;
            return false;
        }
    }
}

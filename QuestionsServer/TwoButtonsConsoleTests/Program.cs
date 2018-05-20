using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //AddWrapper addWrapper = new AddWrapper();
            //if (addWrapper.TryAddUser("", "testAdd5", 1525, 235, 15, "testPhone5", "", "", out int userId))
            //    Console.WriteLine(userId);

            List<int> p = new List<int>();

            //CommentsWrapper commentsWrapper = new CommentsWrapper();
            //if (commentsWrapper.TryGetComments("",1,1,100,out IEnumerable<CommentDb> comments))
            //    Console.WriteLine(comments.FirstOrDefault().Comment);

            using (var db = new TwoButtonsContext(CreateOptions()))
            {
                //var user = db.UserDb.FirstOrDefault();
                //Console.WriteLine(user?.Login);
                //if (AccountWrapper.TryGetIdentification(db, "govjad", "1", out int userId))
                //    Console.WriteLine(userId);
            }

            Console.WriteLine("Hello World!");
        }

        static DbContextOptions<TwoButtonsContext> CreateOptions()
        {
            string connectionString = "Server=DESKTOP-T41QO6T\\SQLEXPRESS;database=TwoButtons;Trusted_Connection=True;";

            var optionsBuilder = new DbContextOptionsBuilder<TwoButtonsContext>();
            return  optionsBuilder
                .UseSqlServer(connectionString)
                .Options;
        }
    }
}

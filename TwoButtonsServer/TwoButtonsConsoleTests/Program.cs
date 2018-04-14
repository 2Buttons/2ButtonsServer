using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.WrapperFunctions;

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
            Console.WriteLine("Hello World!");
        }
    }
}

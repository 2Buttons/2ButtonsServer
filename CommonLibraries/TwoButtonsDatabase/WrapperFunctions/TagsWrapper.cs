using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class TagsWrapper
    {
        public static bool TryAddTag(TwoButtonsContext db, int questionId, string tagText, int position)
        {
            try
            {
                db.Database.ExecuteSqlCommand($"addTag {questionId}, {tagText}, {position}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public static bool TryGetTags(TwoButtonsContext db, int questionId, out IEnumerable<TagDb> tags)
        {

            try
            {
                tags = db.TagDb.FromSql($"select * from dbo.getTags({questionId})").ToList();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            tags =  new List<TagDb>();
            return false;

        }
    }
}
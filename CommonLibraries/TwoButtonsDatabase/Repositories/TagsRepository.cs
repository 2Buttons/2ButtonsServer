using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.Repositories
{
  public class TagsRepository
  {
    private readonly TwoButtonsContext _db;

    public TagsRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public  bool TryAddTag( int questionId, string tagText, int position)
    {
      try
      {
        _db.Database.ExecuteSqlCommand($"addTag {questionId}, {tagText}, {position}");
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public  bool TryGetTags( int questionId, out IEnumerable<TagDb> tags)
    {
      try
      {
        tags = _db.TagDb.FromSql($"select * from dbo.getTags({questionId})").ToList();

        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      tags = new List<TagDb>();
      return false;
    }
  }
}
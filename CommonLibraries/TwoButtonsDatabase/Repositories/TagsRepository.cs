using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public async Task<bool> AddTag(int questionId, string tagText, int position)
    {
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync($"addTag {questionId}, {tagText}, {position}") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public async Task<List<TagDb>> GetTags(int questionId)
    {
      try
      {
        return await _db.TagDb.AsNoTracking().FromSql($"select * from dbo.getTags({questionId})").ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return new List<TagDb>();
    }
  }
}
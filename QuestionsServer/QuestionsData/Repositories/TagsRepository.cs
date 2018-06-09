using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuestionsData.Entities;
using QuestionsData.Queries;

namespace QuestionsData.Repositories
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
      return await _db.Database.ExecuteSqlCommandAsync($"addTag {questionId}, {tagText}, {position}") > 0;
    }

    public async Task<List<TagDb>> GetTags(int questionId)
    {
      return _db.TagDb.AsNoTracking().FromSql($"select * from dbo.getTags({questionId})").ToList();
    }

    public List<TagDb> GetTags(TwoButtonsContext context, int questionId)
    {
      return context.TagDb.AsNoTracking().FromSql($"select * from dbo.getTags({questionId})").ToList();
    }
  }
}
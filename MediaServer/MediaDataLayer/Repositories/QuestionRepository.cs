using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MediaDataLayer.Repositories
{
  public class QuestionRepository
  {
    private readonly TwoButtonsContext _db;

    public QuestionRepository(TwoButtonsContext db)
    {
      _db = db;
    }

    public async Task<bool> UpdateQuestionBackgroundLink(int questionId, string backgroundImageLink)
    {
      try
      {
        return await _db.Database.ExecuteSqlCommandAsync(
                 $"updateQuestionBackground {questionId}, {backgroundImageLink}") > 0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }
  }
}
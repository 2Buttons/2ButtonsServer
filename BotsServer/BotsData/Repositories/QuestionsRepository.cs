using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotsData.Contexts;
using BotsData.Dto;
using CommonLibraries;
using CommonLibraries.Entities.Main;
using Microsoft.EntityFrameworkCore;

namespace BotsData.Repositories
{
  public class QuestionsRepository
  {
    private readonly TwoButtonsContext _context;
    private readonly TwoButtonsAccountContext _contextAccount;

    public QuestionsRepository(TwoButtonsContext context, TwoButtonsAccountContext contextAccount)
    {
      _context = context;
      _contextAccount = contextAccount;
    }

    public async Task<List<QuestionDto>> GetQuestions(int offset, int count)
    {
      var result = await  _context.QuestionEntities.Skip(offset).Take(count).GroupJoin(_context.OptionEntities,
        x => x.QuestionId, y => y.QuestionId,
        (x, y) => new QuestionDto
        {QuestionId  = x.QuestionId,
         UserId = x.UserId,
         OriginalBackgroundUrl = x.OriginalBackgroundUrl,
           AudienceType = x.AudienceType,
            Condition = x.Condition,
             DislikesAmount = x.DislikesCount,
             LikesAmount = x.LikesCount,
              QuestionAddDate = x.AddedDate,
               QuestionType = x.QuestionType,
          Options = y.Select(o => new OptionDto {Text = o.Text, Voters = o.AnswersCount })
        }).ToListAsync();

      return result;
    }

    public async Task<List<QuestionDto>> GetQuestionsByPattern( string pattern,int offset, int count)
    {
      pattern = pattern.ToLower();
      var result = await _context.QuestionEntities.Where(x=>x.Condition.ToLower().Contains(pattern)).Skip(offset).Take(count).GroupJoin(_context.OptionEntities,
        x => x.QuestionId, y => y.QuestionId,
        (x, y) => new QuestionDto
        {
          QuestionId = x.QuestionId,
          UserId = x.UserId,
          OriginalBackgroundUrl = x.OriginalBackgroundUrl,
          AudienceType = x.AudienceType,
          Condition = x.Condition,
          DislikesAmount = x.DislikesCount,
          LikesAmount = x.LikesCount,
          QuestionAddDate = x.AddedDate,
          QuestionType = x.QuestionType,
          Options = y.Select(o => new OptionDto { Text = o.Text, Voters = o.AnswersCount })
        }).ToListAsync();

      return result;
    }

    public async Task<List<QuestionDto>> GetQuestionById(int questionId)
    {
      var result = await _context.QuestionEntities.Where(x=>x.QuestionId == questionId).Take(1).GroupJoin(_context.OptionEntities,
        x => x.QuestionId, y => y.QuestionId,
        (x, y) => new QuestionDto
        {
          QuestionId = x.QuestionId,
          UserId = x.UserId,
          OriginalBackgroundUrl = x.OriginalBackgroundUrl,
          AudienceType = x.AudienceType,
          Condition = x.Condition,
          DislikesAmount = x.DislikesCount,
          LikesAmount = x.LikesCount,
          QuestionAddDate = x.AddedDate,
          QuestionType = x.QuestionType,
          Options = y.Select(o => new OptionDto { Text = o.Text, Voters = o.AnswersCount })
        }).ToListAsync();

      return result;
    }

    public async Task<bool> UpdateAnswer(int userId, int questionId, AnswerType answerType)
    {
      var answered = DateTime.Now;
      return await _context.Database.ExecuteSqlCommandAsync(
               $"updateAnswer {userId}, {questionId}, {answerType}, {answered}") > 0;
    }

    public bool UpdateAnswer(TwoButtonsContext context, int userId, int questionId, AnswerType answerType)
    {
      var answered = DateTime.Now;
     // var m = context.AnswerEntities.ToList();
     // var t = m;
      context.AnswerEntities.Add(new AnswerEntity
      {
        AnswerType = answerType,
        QuestionId = questionId,
        UserId = userId
      });

    //  var p = context.AnswerEntities.ToList();
     // var y = p;

      return context.SaveChanges() > 0;
      //return await _context.Database.ExecuteSqlCommandAsync(
      //         $"updateAnswer {userId}, {questionId}, {answerType}, {answered}") > 0;
    }

    public async  Task<bool> UpdateAnswer(AnswerEntity answer)
    {
      _context.AnswerEntities.Add(answer);


      return await  _context.SaveChangesAsync() > 0;
      //return await _context.Database.ExecuteSqlCommandAsync(
      //         $"updateAnswer {userId}, {questionId}, {answerType}, {answered}") > 0;
    }

    public async Task<bool> UpdateAnswer(TwoButtonsContext context, AnswerEntity answer)
    {
     
      context.AnswerEntities.Add(answer);

      //  var p = context.AnswerEntities.ToList();
      // var y = p;

      return await context.SaveChangesAsync() > 0;
      //return await _context.Database.ExecuteSqlCommandAsync(
      //         $"updateAnswer {userId}, {questionId}, {answerType}, {answered}") > 0;
    }


  }
}
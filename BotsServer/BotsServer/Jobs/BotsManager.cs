using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BotsData;
using BotsData.Contexts;
using BotsServer.ViewModels.Input;
using CommonLibraries;
using CommonLibraries.Entities.Main;
using CommonLibraries.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BotsServer.Jobs
{
  public class BotsManager
  {
    private readonly Random _random = new Random();

    public async Task CreateTimer(BotsUnitOfWork db, DbContextOptions<TwoButtonsContext> dbOptions,
      MagicViewModel magic)
    {
      TimerCallback timerDelegate = Task;

      var errorPercent = _random.Next(3, 5);
      var errorAnwerType = (AnswerType) _random.Next(1, 3);

      var firstOptionPercent = magic.FirstOptionPercent;

      if (errorAnwerType == AnswerType.First) firstOptionPercent += errorPercent;
      else firstOptionPercent -= errorPercent;

      var firstBotsCount = magic.BotsCount * firstOptionPercent / 100;
      var secondBotsCount = magic.BotsCount - firstBotsCount;

      var allBots = await db.BotsRepository.GetAllBotsIdsExceptVoted(magic.QuestionId);
      RandomizerExtension.Shuffle(allBots);

      var list = new List<BotVoting>();

      for (var i = 0; i < firstBotsCount && i < allBots.Count; i++)
        list.Add(new BotVoting {BotId = allBots[i], QuestionId = magic.QuestionId, AnswerType = AnswerType.First});

      for (var i = firstBotsCount; i < secondBotsCount + firstBotsCount && i < allBots.Count; i++)
        list.Add(new BotVoting {BotId = allBots[i], QuestionId = magic.QuestionId, AnswerType = AnswerType.Second});

      RandomizerExtension.Shuffle(list);
      foreach (var bot in list)
      {
        var job = new JobState
        {
          BotId = bot.BotId,
          AnswerType = bot.AnswerType,
          QuestionId = bot.QuestionId,
          DbOptions = dbOptions,
          Db = db
        };

        //await new TaskFactory().StartNew(() => Task(job));
        var timer = new Timer(timerDelegate, job, _random.Next(magic.VoteDurationInMilliseconds), Timeout.Infinite);
      }
    }

    //private void Task(object jobState)
    //{
    //  var job = (JobState)jobState;
    //  if (job.RemainingIteration <= 0) job.Timer.Dispose();
    //  while (job.BotVotings.Count > 1)
    //  {
    //    Thread.Sleep(2000);
    //    var bot = job.BotVotings.Pop();
    //    job.Db.QuestionRepository.UpdateAnswer(bot.BotId, bot.QuestionId, bot.AnswerType);
    //  }

    //  job.RemainingIteration--;
    //}

    private async void Task(object jobState)
    {
      var job = (JobState) jobState;

      var db = new TwoButtonsContext(job.DbOptions); // TODO интересно проверить теорию, что можно и не создавать

      var answered = DateTime.Now;
      // var m = context.AnswerEntities.ToList();
      // var t = m;
      var answer = new AnswerEntity
      {
        AnswerType = job.AnswerType,
        QuestionId = job.QuestionId,
        UserId = job.BotId,
        AnsweredDate = answered,
        IsDeleted = false,
        FeedbackType = QuestionFeedbackType.Like
      };

      await job.Db.QuestionRepository.UpdateAnswer(db, answer);
    }
    //  if (job.RemainingIteration <= 0) job.Timer.Dispose();
    //  var job = (JobState)jobState;
    //{

    //private void Task(object jobState)
    //  var db = new TwoButtonsContext(job.DbOptions);
    //  for (int i = 0; i < job.BotsPerVote; i++)
    //  {

    //    if (job.Index >= job.BotVotings.Count)
    //    {
    //      job.Timer.Dispose();
    //      return;
    //    }

    //    var bot = job.BotVotings[job.Index];
    //    var answered = DateTime.Now;
    //    // var m = context.AnswerEntities.ToList();
    //    // var t = m;
    //    var answer = new AnswerEntity
    //    {
    //      AnswerType = bot.AnswerType,
    //      QuestionId = bot.QuestionId,
    //      UserId = bot.BotId,
    //       AnsweredDate  =  answered,
    //        IsDeleted = 0,
    //         IsLiked = 0

    //    };

    //    job.Db.QuestionRepository.UpdateAnswer(db, answer);
    //    Interlocked.Increment(ref job.Index);
    //  }

    //  job.RemainingIteration--;
    //}
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BotsData;
using BotsData.Contexts;
using BotsData.Entities;
using BotsServer.ViewModels.Input;
using CommonLibraries;
using CommonLibraries.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BotsServer.Jobs
{
  public class BotsManager
  {
  

    private readonly Random _random = new Random();
    public BotsManager()
    {

    }

    public async Task CreateTimer(BotsUnitOfWork db, DbContextOptions<TwoButtonsContext> dbOptions, MagicViewModel magic)
    {
      TimerCallback timerDelegate = Task;
     
    

      var errorPercent = _random.Next(1, 4);
      var errorAnwerType = (AnswerType)_random.Next(1, 3);

      var firstOptionPercent = magic.FirstOptionPercent;
      var secondOptionPercent = magic.SecondOptionPercent;

      if (errorAnwerType == AnswerType.First)
      {
        firstOptionPercent += errorPercent;
        secondOptionPercent -= errorPercent;
      }
      else
      {
        firstOptionPercent -= errorPercent;
        secondOptionPercent += errorPercent;
      }


      var firstBotsCount = magic.BotsCount * firstOptionPercent / 100;
      var secondBotsCount = magic.BotsCount * secondOptionPercent / 100;

      var allBots = await db.BotsRepository.GetAllBotsIdsExceptVoted(magic.QuestionId);
      RandomizerExtension.Shuffle(allBots);
      
      
      List<BotVoting> list = new List<BotVoting>();

      for (int i = 0; i < firstBotsCount && i < allBots.Count; i++)
      {
        list.Add(new BotVoting { BotId = allBots[i], QuestionId = magic.QuestionId, AnswerType = AnswerType.First });
       
      }

      for (int i = firstBotsCount; i < secondBotsCount + firstBotsCount  && i<allBots.Count; i++)
      {
        list.Add(new BotVoting {BotId = allBots[i], QuestionId = magic.QuestionId, AnswerType = AnswerType.Second});
      }

      RandomizerExtension.Shuffle<BotVoting>(list);
      List<BotVoting> stack = new List<BotVoting>(list);

      var job = new JobState { BotVotings = stack , RemainingIteration = list.Count / magic.BotsPerVote , BotsPerVote = magic.BotsPerVote, Db = db, DbOptions = dbOptions, Index =  0};

      //await new TaskFactory().StartNew(() => Task(job));
        var timer = new Timer(timerDelegate, job, 0, magic.IntervalInMilliseconds);
     job.Timer = timer;
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


    private void Task(object jobState)
    {
      var job = (JobState)jobState;
      if (job.RemainingIteration <= 0) job.Timer.Dispose();
      var db = new TwoButtonsContext(job.DbOptions);
      for (int i = 0; i < job.BotsPerVote; i++)
      {
        
        if (job.Index >= job.BotVotings.Count)
        {
          job.Timer.Dispose();
          return;
        }
     
        var bot = job.BotVotings[job.Index];
        var answered = DateTime.Now;
        // var m = context.AnswerEntities.ToList();
        // var t = m;
        var answer = new AnswerEntity
        {
          AnswerType = bot.AnswerType,
          QuestionId = bot.QuestionId,
          UserId = bot.BotId,
           AnsweredDate  =  answered,
            IsDeleted = 0,
             IsLiked = 0
          
        };

        job.Db.QuestionRepository.UpdateAnswer(db, answer);
        Interlocked.Increment(ref job.Index);
      }


      job.RemainingIteration--;
    }

  }
}

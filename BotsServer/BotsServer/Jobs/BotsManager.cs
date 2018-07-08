using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BotsData;
using BotsServer.ViewModels.Input;
using CommonLibraries;
using CommonLibraries.Extensions;

namespace BotsServer.Jobs
{
  public class BotsManager
  {
  
    private readonly Stack<BotVoting> _votings = new Stack<BotVoting>();

    private readonly Random _random = new Random();
    public BotsManager(BotsUnitOfWork db, MagicViewModel magic)
    {

    }

    public async Task CreateTimer(BotsUnitOfWork db, MagicViewModel magic)
    {
      TimerCallback TimerDelegate =
        Task;
     
    

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

      var allBots = await db.BotsRepository.GetAllBotsIds();
      RandomizerExtension.Shuffle(allBots);
      
      
      List<BotVoting> list = new List<BotVoting>();
      for (int i = 0; i < secondBotsCount + firstBotsCount  && i<allBots.Count; i+=2)
      {
        list.Add(new BotVoting { BotId = allBots[i],QuestionId = magic.QuestionId, AnswerType = AnswerType.First});
        list.Add(new BotVoting {BotId = allBots[i], QuestionId = magic.QuestionId, AnswerType = AnswerType.Second});
      }

     
      Stack<BotVoting> stack = new Stack<BotVoting>(list);

      var job = new JobState { BotVotings = stack , RemainingIteration = list.Count / magic.BotsPerVote , BotsPerVote = magic.BotsPerVote};

      var timer = new Timer(TimerDelegate, job, 0, 500);
      job.Timer = timer;
    }


    private void Task(object jobState)
    {
      var job = (JobState) jobState;
      if (job.RemainingIteration <= 0) job.Timer.Dispose();
      for (int i = 0; i < job.BotsPerVote; i++)
      {
        if (job.BotVotings.Count <= 0) job.Timer.Dispose();
        var bot = job.BotVotings.Pop();
        job.Db.QuestionRepository.UpdateAnswer(bot.BotId, bot.QuestionId, bot.AnswerType);
      }
      job.RemainingIteration--;
    }
   
  }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotsData;
using CommonLibraries;
using Quartz;

namespace BotsServer.Jobs
{
  public class ChangeStatistics : IJob
  {
    private readonly int _interval;
    private readonly BotsUnitOfWork _db;
    private readonly Stack<BotVoting> _votings = new Stack<BotVoting>();
    private readonly int _botsPerVote;
    private readonly int _questionId;
   

    public ChangeStatistics()
    {
   
    }

    public async Task Execute(IJobExecutionContext context)
    {
      
      for (int i = 0; i < _botsPerVote; i++)
      {
        var bot = _votings.Pop();
        await _db.QuestionRepository.UpdateAnswer(bot.BotId, _questionId, bot.AnswerType);
      }
      
    }
  }

  
}

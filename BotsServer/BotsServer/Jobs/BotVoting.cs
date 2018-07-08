using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;

namespace BotsServer.Jobs
{
  public class BotVoting
  {
    public int BotId { get; set; }
    public int QuestionId { get; set; }
    public AnswerType AnswerType { get; set; }
  }
}

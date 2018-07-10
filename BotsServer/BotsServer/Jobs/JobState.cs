using System.Collections.Generic;
using System.Threading;
using BotsData;
using BotsData.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BotsServer.Jobs
{
  public class JobState
  {
    public int RemainingIteration { get; set; }
    public int BotsPerVote { get; set; }
    public BotsUnitOfWork Db { get; set; }
    public Timer Timer { get; set; }
    public List<BotVoting> BotVotings { get; set; }
    public int Index;
    public DbContextOptions<TwoButtonsContext> DbOptions { get; set; }
  }
}
using System;
using System.Threading.Tasks;
using BotsData;
using Quartz;

namespace BotsServer.Jobs
{
  public class ChangeStatistics : IJob
  {
    private readonly int _interval;
    private readonly BotsUnitOfWork _db;

    public ChangeStatistics()
    {
   
    }

    public Task Execute(IJobExecutionContext context)
    {
      throw new NotImplementedException();
    }
  }
}

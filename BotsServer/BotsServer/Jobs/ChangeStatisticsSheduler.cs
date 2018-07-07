using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Quartz;
using Quartz.Impl;

namespace BotsServer.Jobs
{
    public class ChangeStatisticsSheduler
    {
      public static async void Start()
      {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<ChangeStatistics>().Build();

        ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
          .WithIdentity("ChangeStatistics", "Group1")     // идентифицируем триггер с именем и группой
          .StartNow()                            // запуск сразу после начала выполнения
          .WithSimpleSchedule(x => x            // настраиваем выполнение действия
            .WithIntervalInMinutes(1) // через 1 минуту
            .WithRepeatCount(10))                   // бесконечное повторение
          .Build();                               // создаем триггер

        await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
      }
    }
}


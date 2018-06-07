using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.Exceptions.ApiExceptions;
using Microsoft.EntityFrameworkCore;
using MonitoringData.Entities;

namespace MonitoringData.Repositories
{
  public class MonitoringRepository
  {
    private readonly TwoButtonsContext _db;

    public MonitoringRepository(TwoButtonsContext db)
    {
      _db = db;
    }


    public async Task<bool> AddUserMonitoring(UrlMonitoringDb userMonitoring)
    {
      _db.UrlMonitoringsDb.Add(userMonitoring);
      return await _db.SaveChangesAsync() > 0;
    }


    public async Task<bool> UpdateUserMonitoring(int userId, UrlMonitoringType monitoringType)
    {
      var userMonitoring = await _db.UrlMonitoringsDb.FirstOrDefaultAsync(x => x.UserId == userId);
      if (userMonitoring == null) throw new NotFoundException("We can not find this user");

      switch (monitoringType)
      {
        case UrlMonitoringType.GetsQuestionsUserAsked:
          userMonitoring.GetsQuestionsUserAsked++;
          break;
        case UrlMonitoringType.GetsQuestionsUserAnswered:
          userMonitoring.GetsQuestionsUserAnswered++;
          break;
        case UrlMonitoringType.GetsQuestionsUserFavorite:
          userMonitoring.GetsQuestionsUserFavorite++;
          break;
        case UrlMonitoringType.GetsQuestionsUserCommented:
          userMonitoring.GetsQuestionsUserCommented++;
          break;
        case UrlMonitoringType.GetsQuestionsPersonalAsked:
          userMonitoring.GetsQuestionsPersonalAsked++;
          break;
        case UrlMonitoringType.GetsQuestionsPersonalRecommended:
          userMonitoring.GetsQuestionsPersonalRecommended++;
          break;
        case UrlMonitoringType.GetsQuestionsPersonalChosen:
          userMonitoring.GetsQuestionsPersonalChosen++;
          break;
        case UrlMonitoringType.GetsQuestionsPersonalLiked:
          userMonitoring.GetsQuestionsPersonalLiked++;
          break;
        case UrlMonitoringType.GetsQuestionsPersonalSaved:
          userMonitoring.GetsQuestionsPersonalSaved++;
          break;
        case UrlMonitoringType.GetsQuestionsPersonalDayTop:
          userMonitoring.GetsQuestionsPersonalDayTop++;
          break;
        case UrlMonitoringType.GetsQuestionsPersonalWeekTop:
          userMonitoring.GetsQuestionsPersonalWeekTop++;
          break;
        case UrlMonitoringType.GetsQuestionsPersonalMonthTop:
          userMonitoring.GetsQuestionsPersonalMonthTop++;
          break;
        case UrlMonitoringType.GetsQuestionsPersonalAllTimeTop:
          userMonitoring.GetsQuestionsPersonalAllTimeTop++;
          break;
        case UrlMonitoringType.GetsQuestionsNews:
          userMonitoring.GetsQuestionsNews++;
          break;
        case UrlMonitoringType.OpensPersonalPage:
          userMonitoring.OpensPersonalPage++;
          break;
        case UrlMonitoringType.OpensUserPage:
          userMonitoring.OpensUserPage++;
          break;
        case UrlMonitoringType.GetsNotifications:
          userMonitoring.GetsNotifications++;
          break;
        case UrlMonitoringType.FiltersQuestions:
          userMonitoring.FiltersQuestions++;
          break;
        case UrlMonitoringType.OpensQuestionPage:
          userMonitoring.OpensQuestionPage++;
          break;
        case UrlMonitoringType.GetsComments:
          userMonitoring.GetsComments++;
          break;
      }
      _db.UrlMonitoringsDb.Update(userMonitoring);
      return await _db.SaveChangesAsync() > 0;
    }


  }
}
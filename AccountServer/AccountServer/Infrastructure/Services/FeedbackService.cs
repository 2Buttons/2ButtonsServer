using System.Collections.Generic;
using System.Threading.Tasks;
using AccountData;
using CommonLibraries.Entities.Main;
using Microsoft.Extensions.Logging;

namespace AccountServer.Infrastructure.Services
{
  public class FeedbackService : IFeedbackService
  {
    private readonly AccountDataUnitOfWork _db;
    private readonly ILogger<FeedbackService> _logger;

    public FeedbackService(AccountDataUnitOfWork accountDb, ILogger<FeedbackService> logger)
    {
      _db = accountDb;
      _logger = logger;
    }

    public async Task<bool> AddFeedback(FeedbackEntity feedback)
    {
      _logger.LogInformation($"{nameof(CityService)}.{nameof(AddFeedback)}.Start");
      var result =  await _db.Feedbacks.AddFeedbackAsync(feedback);
      _logger.LogInformation($"{nameof(CityService)}.{nameof(AddFeedback)}.End");
      return result;

    }

    public async Task<List<FeedbackEntity>> GetUserFeedbacks(int userId)
    {
      _logger.LogInformation($"{nameof(CityService)}.{nameof(GetUserFeedbacks)}.Start");
      var result =  await _db.Feedbacks.GetFeedbacksByUserAsync(userId);
      _logger.LogInformation($"{nameof(CityService)}.{nameof(GetUserFeedbacks)}.End");
      return result;
    }

    public async Task<List<FeedbackEntity>> GetFeedbacks(int offset, int count)
    {
      _logger.LogInformation($"{nameof(CityService)}.{nameof(GetFeedbacks)}.Start");
      var result =  await _db.Feedbacks.GetFeedbacks(offset, count);
      _logger.LogInformation($"{nameof(CityService)}.{nameof(GetFeedbacks)}.End");
      return result;
    }
  }
}
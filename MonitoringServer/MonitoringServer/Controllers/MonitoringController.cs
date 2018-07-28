using System.Net;
using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonitoringData;
using MonitoringData.Entities;
using MonitoringServer.ViewModels;

namespace MonitoringServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("monitoring")]
  public class MonitoringController : ControllerBase
  {
    private readonly MonitoringUnitOfWork _db;
    private readonly ILogger<MonitoringController> _logger;

    public MonitoringController(MonitoringUnitOfWork db, ILogger<MonitoringController> logger)
    {
      _db = db;
      _logger = logger;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddMonitoring([FromBody] AddUrlMonitoring urlMonitoring)
    {
      _logger.LogInformation($"{nameof(MonitoringController)}.{nameof(AddMonitoring)}.Start");
      var monitoring = new UrlMonitoringDb {UserId = urlMonitoring.UserId};
      var result = await _db.Monitorings.AddUserMonitoring(monitoring);
      _logger.LogInformation($"{nameof(MonitoringController)}.{nameof(AddMonitoring)}.End");
      return result ? new OkResponseResult(new { IsAdded = true }) : new ResponseResult((int)HttpStatusCode.InternalServerError, "We can not add monitoring", new { IsAdded = false });
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateMonitoring([FromBody] UpdateUrlMonitoring urlMonitoring)
    {
      if (urlMonitoring.UserId == 0)
      {
        if (!await _db.Monitorings.AddIfNotExistUserMonitoring(new UrlMonitoringDb {UserId = urlMonitoring.UserId}))
          return new ResponseResult((int) HttpStatusCode.InternalServerError, "We can not add monitoring");
      }
      _logger.LogInformation($"{nameof(MonitoringController)}.{nameof(UpdateMonitoring)}.Start");
      var result = await _db.Monitorings.UpdateUserMonitoring(urlMonitoring.UserId, urlMonitoring.UrlMonitoringType);
      _logger.LogInformation($"{nameof(MonitoringController)}.{nameof(UpdateMonitoring)}.End");
      return result ? new OkResponseResult(new { IsAdded = true }) : new ResponseResult((int)HttpStatusCode.InternalServerError, "We can not add monitoring", new { IsAdded = false });
    }
  }
}
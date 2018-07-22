using System.Net;
using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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

    public MonitoringController(MonitoringUnitOfWork db)
    {
      _db = db;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddMonitoring([FromBody] AddUrlMonitoring urlMonitoring)
    {
      var monitoring = new UrlMonitoringDb {UserId = urlMonitoring.UserId};
      var result = await _db.Monitorings.AddUserMonitoring(monitoring);
      return result ? new OkResponseResult(new { IsAdded = result }) : new ResponseResult((int)HttpStatusCode.InternalServerError, "We can not add monitoring");
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateMonitoring([FromBody] UpdateUrlMonitoring urlMonitoring)
    {
      if (urlMonitoring.UserId == 0)
      {
        if (!await _db.Monitorings.AddIfNotExistUserMonitoring(new UrlMonitoringDb {UserId = urlMonitoring.UserId}))
          return new ResponseResult((int) HttpStatusCode.InternalServerError, "We can not add monitoring");
      }
      var result = await _db.Monitorings.UpdateUserMonitoring(urlMonitoring.UserId, urlMonitoring.UrlMonitoringType);
      return result ? new OkResponseResult(new {IsAdded = result}) : new ResponseResult((int)HttpStatusCode.InternalServerError, "We can not add monitoring");
    }
  }
}
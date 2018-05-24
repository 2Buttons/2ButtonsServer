using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountData;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using CommonLibraries.ApiResponse;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("api/[controller]")]
  public class NotificationsController : Controller //To get user's posts
  {
    private readonly AccountDataUnitOfWork _db;

    public NotificationsController(AccountDataUnitOfWork db)
    {
      _db = db;
    }

    [HttpPost("notifications")]
    public async Task<IActionResult> GetNotifications([FromBody] UserIdViewModel userId)
    {
      if (userId == null)
        return new BadResponseResult($"Input body is null");

      var notifications = await _db.Notifications.GetNotifications(userId.UserId);
      //  return new BadResponseResult("Something goes wrong. We will fix it!... maybe)))");

      return new OkResponseResult(notifications.MapNotificationDbToViewModel());
    }
  }
}

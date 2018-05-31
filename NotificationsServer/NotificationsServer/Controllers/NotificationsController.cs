using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NotificationsData;
using NotificationServer.ViewModels;

namespace NotificationServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  public class NotificationsController : Controller
  {
    private readonly NotificationsDataUnitOfWork _db;

    public NotificationsController(NotificationsDataUnitOfWork db)
    {
      _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> GetNotifications([FromBody] UserIdViewModel userId)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var notifications = await _db.Notifications.GetNotifications(userId.UserId);
      return new OkResponseResult(notifications.MapNotificationDbToViewModel());
    }
  }
}
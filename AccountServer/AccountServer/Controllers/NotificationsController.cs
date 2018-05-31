using System.Threading.Tasks;
using AccountData;
using AccountServer.Infrastructure.Services;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AccountServer.Controllers
{
  //[EnableCors("AllowAllOrigin")]
  //[Produces("application/json")]
  [Route("api/")]
  public class NotificationsController : Controller //To get user's posts
  {
    private readonly AccountDataUnitOfWork _db;
    private readonly NotificationsMessageHandler _notificationsMessageHandler;

    public NotificationsController(AccountDataUnitOfWork db, NotificationsMessageHandler notificationsMessageHandler)
    {
      _db = db;
      _notificationsMessageHandler = notificationsMessageHandler;
    }

    [HttpPost("notifications1")]
    public async Task<IActionResult> GetNotifications([FromBody] UserIdViewModel userId)
    {
      if(!ModelState.IsValid)
        return new BadResponseResult(ModelState);
     
      var notifications = await _db.Notifications.GetNotifications(userId.UserId);
      return new OkResponseResult(notifications.MapNotificationDbToViewModel());
    }

    [HttpGet("sendmessage")]
    public async Task SendMessage([FromQueryAttribute]string message)
    {
      await _notificationsMessageHandler.SendMessageToAllAsync(message);
    }
  }
}
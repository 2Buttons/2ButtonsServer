using System.Threading.Tasks;
using AccountData;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using CommonLibraries.MediaFolders;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  public class NotificationsController : Controller //To get user's posts
  {
    private AccountDataUnitOfWork Db { get; }
    private MediaConverter MediaConverter { get; }

    public NotificationsController(AccountDataUnitOfWork db, MediaConverter mediaConverter)
    {
      Db = db;
      MediaConverter = mediaConverter;
    }

    [HttpPost("account/notifications")]
    public async Task<IActionResult> GetNotifications([FromBody] UserIdViewModel userId)
    {
      if(!ModelState.IsValid)
        return new BadResponseResult(ModelState);
     
      var notifications = await Db.Notifications.GetNotifications(userId.UserId);
      return new OkResponseResult(notifications.MapNotificationDbToViewModel(MediaConverter));
    }
  }
}
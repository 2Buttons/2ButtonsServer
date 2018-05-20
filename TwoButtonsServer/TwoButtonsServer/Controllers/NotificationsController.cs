using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuestionsData;
using QuestionsServer.ViewModels.InputParameters;
using QuestionsServer.ViewModels.OutputParameters;

namespace QuestionsServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("api/[controller]")]
  public class NotificationsController : Controller //To get user's posts
  {
    private readonly TwoButtonsUnitOfWork _mainDb;

    public NotificationsController(TwoButtonsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    [HttpPost("notifications")]
    public async Task<IActionResult> GetNotifications([FromBody] UserIdViewModel userId)
    {
      if (userId == null)
        return BadRequest($"Input parameter  is null");

      var notifications = await _mainDb.Notifications.GetNotifications(userId.UserId);
      //  return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      return Ok(notifications.MapNotificationDbToViewModel());
    }
  }
}
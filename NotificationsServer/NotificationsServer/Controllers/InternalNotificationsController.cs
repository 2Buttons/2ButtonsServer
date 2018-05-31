using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Mvc;
using NotificationsData;
using NotificationServer.ViewModels;
using NotificationServer.ViewModels.Input;

namespace NotificationServer.Controllers
{

      //[EnableCors("AllowAllOrigin")]
      //[Produces("application/json")]
      [Route("internal")]
      public class InternalNotificationsController : Controller //To get user's posts
      {
        private readonly NotificationsDataUnitOfWork _db;
        private readonly NotificationsMessageHandler _notificationsMessageHandler;

        public InternalNotificationsController(NotificationsDataUnitOfWork db, NotificationsMessageHandler notificationsMessageHandler)
        {
          _db = db;
          _notificationsMessageHandler = notificationsMessageHandler;
        }

        [HttpPost("follow")]
        public async Task<IActionResult> GetNotifications([FromBody] FollowNotification notification)
        {
          if (!ModelState.IsValid)
            return new BadResponseResult(ModelState);

          var notifications = await _db.Notifications.GetNotifications(userId.UserId);
          return new OkResponseResult(notifications.MapNotificationDbToViewModel());
        }

        [HttpPost("recommendQuestion")]
        public async Task<IActionResult> GetNotifications([FromBody] RecommendedQuestionNotification notification)
        {
          if (!ModelState.IsValid)
            return new BadResponseResult(ModelState);

          var notifications = await _db.Notifications.GetNotifications(userId.UserId);
          return new OkResponseResult(notifications.MapNotificationDbToViewModel());
        }

        [HttpPost("answer")]
        public async Task<IActionResult> GetNotifications([FromBody] AnswerNotification notification)
        {
          if (!ModelState.IsValid)
            return new BadResponseResult(ModelState);

          var notifications = await _db.Notifications.GetNotifications(userId.UserId);
          return new OkResponseResult(notifications.MapNotificationDbToViewModel());
        }

    [HttpGet("sendmessage")]
        public async Task SendMessage([FromQuery]string message)
        {
          await _notificationsMessageHandler.SendMessageToAllAsync(message);
        }
      }
  
}

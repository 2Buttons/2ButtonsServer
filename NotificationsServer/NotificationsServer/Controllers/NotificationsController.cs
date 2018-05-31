using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Mvc;
using NotificationsData;

namespace NotificationsServer.Controllers
{

      //[EnableCors("AllowAllOrigin")]
      //[Produces("application/json")]
      [Route("api/")]
      public class NotificationsController : Controller //To get user's posts
      {
        private readonly NotificationsDataUnitOfWork _db;
        private readonly NotificationsMessageHandler _notificationsMessageHandler;

        public NotificationsController(NotificationsDataUnitOfWork db, NotificationsMessageHandler notificationsMessageHandler)
        {
          _db = db;
          _notificationsMessageHandler = notificationsMessageHandler;
        }

        [HttpPost("notifications1")]
        public async Task<IActionResult> GetNotifications([FromBody] UserIdViewModel userId)
        {
          if (!ModelState.IsValid)
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

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.WrapperFunctions;
using TwoButtonsServer.ViewModels;
using TwoButtonsServer.ViewModels.InputParameters;
using TwoButtonsServer.ViewModels.OutputParameters;
using TwoButtonsServer.ViewModels.OutputParameters.UserQuestions;

namespace TwoButtonsServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class NotificationsController : Controller //To get user's posts
    {
        private readonly TwoButtonsContext _context;
        public NotificationsController(TwoButtonsContext context)
        {
            _context = context;
        }

        [HttpPost("getNotifications")]
        public IActionResult GetNotifications([FromBody]UserIdViewModel userId)
        {
            if (userId == null)
                return BadRequest($"Input parameter  is null");

            if (!NotificationsWrapper.TryGetNotifications(_context, userId.UserId, out var notifications))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");
           
            return Ok(notifications.MapNotificationDbToViewModel());
        }

        
    }
}
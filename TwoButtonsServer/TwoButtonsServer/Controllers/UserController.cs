﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;

namespace TwoButtonsServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class UserController : Controller  //We get user's information
    {
        private readonly TwoButtonsContext _context;
        public UserController(TwoButtonsContext context)
        {
            _context = context;
        }
        // GET api/getUserInfo/1/25
        [HttpPost("getUserInfo")]
        public IActionResult GetUserInfo(int id, int userId)
        {
            if (UserWrapper.TryGetUserInfo(_context, id, userId, out var userInfo))
                return Ok(userInfo);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }
    }
}
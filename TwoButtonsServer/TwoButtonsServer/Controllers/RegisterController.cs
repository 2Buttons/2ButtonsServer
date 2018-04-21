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
    public class RegisterController : Controller //Controller for a registration
    {
        private readonly TwoButtonsContext _context;
        public RegisterController(TwoButtonsContext context)
        {
            _context = context;
        }
        // GET api/addUser/
        [HttpPost("addUser")]
        public IActionResult AddUser(string login, string password, int age, int sex, string phone = null, string description = null, string fullAvatarLink = null, string smallAvatarLink = null)
        {
            if (UserWrapper.TryAddUser(_context, login, password, age, sex, phone, description, fullAvatarLink, smallAvatarLink, out var userId))
                return Ok(userId);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }
    }
}
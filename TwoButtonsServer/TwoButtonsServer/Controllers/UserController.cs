using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;
using TwoButtonsServer.ViewModels;

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

        [HttpPost("addUser")]
        public IActionResult AddUser(string login, string password, int age, int sex, string phone = null, string description = null, string fullAvatarLink = null, string smallAvatarLink = null)
        {
            if (UserWrapper.TryAddUser(_context, login, password, age, sex, phone, description, fullAvatarLink, smallAvatarLink, out var userId))
                return Ok(userId);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("getUserInfo")]
        public IActionResult GetUserInfo(int id, int userId)
        {
            if (!UserWrapper.TryGetUserInfo(_context, id, userId, out var userInfo))
                return BadRequest("Something goes wrong in TryGetUserInfo. We will fix it!... maybe)))");
            if (!UserWrapper.TryGetUserStatistics(_context, id, out var userStatistics))
                return BadRequest("Something goes wrong in TryGetUserStatistics. We will fix it!... maybe)))");
            if (!UserWrapper.TryGetUserContacts(_context, id, out var userContacts))
                return BadRequest("Something goes wrong in TryGetUserContacts. We will fix it!... maybe)))");


            var result = userInfo.MapToUserInfoViewModel(userStatistics, userContacts);

            return Ok(result);
        }
    }
}
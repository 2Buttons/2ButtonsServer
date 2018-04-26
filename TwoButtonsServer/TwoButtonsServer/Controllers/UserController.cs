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
using TwoButtonsServer.ViewModels.InputParameters;
using TwoButtonsServer.ViewModels.OutputParameters;

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
        public IActionResult AddUser([FromBody]UserRegistrationViewModel user)
        {
            if (user == null)
                return BadRequest($"Input parameter  is null");

            if (UserWrapper.TryAddUser(_context, user.Login, user.Password, user.Age, user.Sex, user.Phone, user.Description, user.FullAvatarLink, user.SmallAvatarLink, out var userId))
                return Ok(userId);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("getUserInfo")]
        public IActionResult GetUserInfo([FromBody]UserPageIdViewModel userPage)
        {
            if (userPage == null)
                return BadRequest($"Input parameter  is null");

            if (!UserWrapper.TryGetUserInfo(_context, userPage.UserId, userPage.UserPageId, out var userInfo))
                return BadRequest("Something goes wrong in TryGetUserInfo. We will fix it!... maybe)))");
            if (!UserWrapper.TryGetUserStatistics(_context, userPage.UserId, out var userStatistics))
                return BadRequest("Something goes wrong in TryGetUserStatistics. We will fix it!... maybe)))");
            if (!UserWrapper.TryGetUserContacts(_context, userPage.UserId, out var userContacts))
                return BadRequest("Something goes wrong in TryGetUserContacts. We will fix it!... maybe)))");


            var result = userInfo.MapToUserInfoViewModel(userStatistics, userContacts);

            return Ok(result);
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;
using TwoButtonsServer.ViewModels.InputParameters;

namespace TwoButtonsServer.Controllers
{
    [Produces("application/json")]
    [EnableCors("AllowAllOrigin")]
    //[Route("api/[controller]")]
    public class FollowersController : Controller //Represent user's followers or people who user are follow to
    {

        private readonly TwoButtonsContext _context;
        public FollowersController(TwoButtonsContext context)
        {
            _context = context;
        }

        [HttpPost("getFollowers")]
        public IActionResult GetFollowers([FromBody]FollowerViewModel vm)
        {
            if (PeopleListWrapper.TryGetFollowers(_context, vm.UserId, vm.UserPageId, vm.Amount, vm.SearchedLogin, out var followers))
                return Ok(followers);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("getFollowTo")]
        public IActionResult GetFollowTo([FromBody]FollowerViewModel vm)
        {
            if (PeopleListWrapper.TryGetFollowTo(_context, vm.UserId, vm.UserPageId, vm.Amount, vm.SearchedLogin, out var follower))
                return Ok(follower);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

    }
}
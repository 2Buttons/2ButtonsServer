using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;

namespace TwoButtonsServer.Controllers
{
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class FollowersController : Controller //Represent user's followers or people who user are follow to
    {

        private readonly TwoButtonsContext _context;
        public FollowersController(TwoButtonsContext context)
        {
            _context = context;
        }
        // GET api/getFollowers/1/25
        [HttpGet("getFollowers")]
        public IActionResult GetFollowers(int id, int userId, int amount=100, string search="")
        {
            if (PeopleListWrapper.TryGetFollowers(_context, id, userId, amount, search, out var followers))
                return Ok(followers);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }
        // GET api/getFollowTo/1/25
        [HttpGet("getFollowTo")]
        public IActionResult GetFollowTo(int id, int userId, int amount=100, string search="")
        {
            if (PeopleListWrapper.TryGetFollowTo(_context,id,userId,amount,search, out var follower))
                return Ok(follower);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

    }
}
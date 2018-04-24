using System;
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
    public class PostsController : Controller //To get user's posts
    {
        private readonly TwoButtonsContext _context;
        public PostsController(TwoButtonsContext context)
        {
            _context = context;
        }


        [HttpPost("getPosts")]
        public IActionResult GetPosts([FromBody]GetPostsViewModel getPosts)
        {
            if (UserWrapper.TryGetPosts(_context, getPosts.UserId, getPosts.UserPageId, getPosts.PostsAmount, out var posts))
                return Ok(posts);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }


       
    }
}
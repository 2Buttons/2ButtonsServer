using System;
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
    public class MarkersController : Controller //Don't receive deleted
    {
        private readonly TwoButtonsContext _context;
        public MarkersController(TwoButtonsContext context)
        {
            _context = context;
        }
        // GET api/getAskedQuestions/1/
        [HttpGet("getAskedQuestions")]
        public IActionResult GetAskedQuestions(int id, int amount = 100, string search = "")
        {
            if (UserQuestionsWrapper.TryGetUserAskedQuestions(_context,id,id,amount,true,out var userAskedQuestions))
                return Ok(userAskedQuestions);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        // GET api/getFavoriteQuestions/1/
        [HttpGet("getFavoriteQuestions")]
        public IActionResult GetFavoriteQuestions(int id, int amount = 100, string search = "")
        {
            if (UserQuestionsWrapper.TryGetUserFavoriteQuestions(_context, id, id, amount, true, out var userAskedQuestions))
                return Ok(userAskedQuestions);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        // GET api/getAnsweredQuestions/1/
        [HttpGet("getAnsweredQuestions")]
        public IActionResult GetAnsweredQuestions(int id, int amount=100, string search="")
        {
            if (UserQuestionsWrapper.TryGetUserAnsweredQuestions(_context, id, id, amount, true, out var userAskedQuestions))
                return Ok(userAskedQuestions);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }
    }
}
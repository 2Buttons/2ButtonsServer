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
    public class QuestionsController : Controller //Controller for a Question
    {
        private readonly TwoButtonsContext _context;
        public QuestionsController(TwoButtonsContext context)
        {
            _context = context;
        }
        // GET api/saveFeedback/
        [HttpPost("saveFeedback")]
        public IActionResult SaveFeedback(int id, int questionId, int feedback=0)
        {
            if (QuestionWrapper.TrySaveFeedback(_context,id, questionId, feedback))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        // GET api/saveFeedback/
        [HttpPost("saveFavorites")]
        public IActionResult SaveFavorites(int id, int questionId, int inFavorites=0)
        {
            if (QuestionWrapper.TrySaveFeedback(_context, id, questionId, inFavorites))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        // GET api/saveFeedback/
        [HttpPost("saveAnswer")]
        public IActionResult SaveAnswer(int id, int questionId, int answer)
        {
            if (QuestionWrapper.TrySaveFeedback(_context, id, questionId, answer))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }
    }
}
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
    public class VotersController : Controller //Controller for question's voters
    {
        private readonly TwoButtonsContext _context;
        public VotersController(TwoButtonsContext context)
        {
            _context = context;
        }
        // GET api/getVoters/
        [HttpPost("getVoters")]
        public IActionResult GetVoters(int id, int questionId, int amount, int option, int minAge = 0, int maxAge = 100, int sex = 0, string search = "")
        {
            if (ResultsWrapper.TryGetAnsweredList(_context, id, questionId, amount, option, minAge, maxAge, sex, search, out var answeredList))
                return Ok(answeredList);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }
    }
}
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
    public class ResultsController : Controller //To work with questions results
    {
        private readonly TwoButtonsContext _context;
        public ResultsController(TwoButtonsContext context)
        {
            _context = context;
        }
        // GET api/getResults/
        [HttpGet("getResults")]
        public IActionResult SaveFeedback(int id, int questionId, int minAge=0, int maxAge=100, int sex=0)
        {
            if (ResultsWrapper.TryGetResults(_context, id, questionId, minAge,maxAge,sex, out var results))
                return Ok(results);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }
    }
}
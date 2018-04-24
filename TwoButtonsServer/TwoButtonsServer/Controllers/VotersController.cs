using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;

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
        //[HttpPost("getVoters")]
        //public IActionResult GetVoters(GetVoters voters)
        //{
        //    if (ResultsWrapper.TryGetAnsweredList(_context, voters.UserId, voters.QuestionId, voters.VotersAmount, voters.Option, voters.MinAge, voters.MaxAge, voters.Sex, voters.SearchedLogin, out var answeredList))
        //        return Ok(answeredList);
        //    return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        //}
    }
}
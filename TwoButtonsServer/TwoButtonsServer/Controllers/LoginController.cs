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
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class LoginController : Controller //Check user's credentials during authentication and send id.
    {
        private readonly TwoButtonsContext _context;
        public LoginController(TwoButtonsContext context)
        {
            _context = context;
        }

        // GET api/login/
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginViewModel login)
        {
            if (LoginWrapper.TryGetIdentification(_context, login.Login, login.Password, out var userId))
                return Ok(userId);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        // GET api/checkValidLogin/
        [HttpPost("checkValidLogin")]
        public IActionResult CheckValidLogin([FromBody]LoginViewModel login)
        {
            if (LoginWrapper.TryCheckValidLogin(_context, login.Login, out var isValid ))
                return Ok(isValid);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }
    }
}
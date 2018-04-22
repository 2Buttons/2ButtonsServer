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
    public class LoginController : Controller //Check user's credentials during authentication and send id.
    {
        private readonly TwoButtonsContext _context;
        public LoginController(TwoButtonsContext context)
        {
            _context = context;
        }

        // GET api/login/
        [HttpPost("login")]
        public IActionResult Login(string login, string password)
        {
            if (LoginWrapper.TryGetIdentification(_context, login, password,out var userId))
                return Ok(userId);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        // GET api/checkValidLogin/
        [HttpPost("checkValidLogin")]
        public IActionResult CheckValidLogin(string login)
        {
            if (LoginWrapper.TryCheckValidLogin(_context, login, out var isValid ))
                return Ok(isValid);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }
    }
}
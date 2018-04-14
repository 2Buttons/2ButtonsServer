using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;

namespace TwoButtonsServer.Controllers
{
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
        [HttpGet("login")]
        public IActionResult Login(string login, string password)
        {
            return Ok(true);
        }

        // GET api/checkValidLogin/
        [HttpGet("checkValidLogin")]
        public IActionResult CheckValidLogin(string login)
        {
            return Ok(true);
        }
    }
}
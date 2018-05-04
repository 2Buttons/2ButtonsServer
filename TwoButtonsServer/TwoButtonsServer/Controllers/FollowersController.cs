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
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;
using TwoButtonsServer.ViewModels.OutputParameters;

namespace TwoButtonsServer.Controllers
{
  [Produces("application/json")]
  [EnableCors("AllowAllOrigin")]
  //[Route("api/[controller]")]
  public class FollowersController : Controller //Represent user's followers or people who user are follow to
  {

    private readonly TwoButtonsContext _context;
    public FollowersController(TwoButtonsContext context)
    {
      _context = context;
    }

    [HttpPost("getFollowers")]
    public IActionResult GetFollowers([FromBody]FollowerViewModel vm)
    {
      if (vm == null || vm.PageParams == null)
        return BadRequest($"Input parameter {nameof(vm)} is null");
      if (FollowersWrapper.TryGetFollowers(_context, vm.UserId, vm.UserPageId, vm.PageParams.Page, vm.PageParams.Amount, vm.SearchedLogin, out var followers))
        return Ok(followers.MapToUserContactsViewModel());
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("getFollowTo")]
    public IActionResult GetFollowTo([FromBody]FollowerViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter {nameof(vm)} is null");

      if (FollowersWrapper.TryGetFollowTo(_context, vm.UserId, vm.UserPageId, vm.PageParams.Page, vm.PageParams.Amount, vm.SearchedLogin, out var follower))
        return Ok(follower.MapToUserContactsViewModel());
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("addFollow")]
    public IActionResult AddFollow([FromBody]FollowViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter is null");

      if (FollowersWrapper.TryAddFollow(_context, vm.FollowerId, vm.FollowToId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("deleteFollow")]
    public IActionResult DeleteFollow([FromBody]FollowViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter is null");

      if (FollowersWrapper.TryDeleteFollow(_context, vm.FollowerId, vm.FollowToId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

  }
}
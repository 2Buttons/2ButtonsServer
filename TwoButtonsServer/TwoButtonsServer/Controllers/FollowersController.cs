using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Repositories;
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

    private readonly TwoButtonsUnitOfWork _mainDb;
    public FollowersController(TwoButtonsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    [HttpPost("getFollowers")]
    public IActionResult GetFollowers([FromBody]FollowerViewModel vm)
    {
      if (vm?.PageParams == null)
        return BadRequest($"Input parameter {nameof(vm)} is null");
      if (_mainDb.Followers.TryGetFollowers(vm.UserId, vm.UserPageId, vm.PageParams.Offset, vm.PageParams.Count, vm.SearchedLogin, out var followers))
        return Ok(followers.MapToUserContactsViewModel());
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("getFollowTo")]
    public IActionResult GetFollowTo([FromBody]FollowerViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter {nameof(vm)} is null");

      if (_mainDb.Followers.TryGetFollowTo(vm.UserId, vm.UserPageId, vm.PageParams.Offset, vm.PageParams.Count, vm.SearchedLogin, out var follower))
        return Ok(follower.MapToUserContactsViewModel());
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("follow")]
    public IActionResult Follow([FromBody]FollowViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter is null");

      if (_mainDb.Followers.TryAddFollow(vm.FollowerId, vm.FollowToId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("unfollow")]
    public IActionResult Unfollow([FromBody]FollowViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter is null");

      if (_mainDb.Followers.TryDeleteFollow(vm.FollowerId, vm.FollowToId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

  }
}
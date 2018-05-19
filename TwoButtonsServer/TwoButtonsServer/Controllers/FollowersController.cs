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
    public async Task<IActionResult> GetFollowers([FromBody]FollowerViewModel vm)
    {
      if (vm?.PageParams == null)
        return BadRequest($"Input parameter {nameof(vm)} is null");
      var followers = await _mainDb.Followers.GetFollowers(vm.UserId, vm.UserPageId, vm.PageParams.Offset,
        vm.PageParams.Count, vm.SearchedLogin);
        return Ok(followers.MapToUserContactsViewModel());
     // return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("getFollowTo")]
    public async Task<IActionResult> GetFollowTo([FromBody]FollowerViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter {nameof(vm)} is null");

      var follower = await _mainDb.Followers.GetFollowTo(vm.UserId, vm.UserPageId, vm.PageParams.Offset, vm.PageParams.Count, vm.SearchedLogin);
        return Ok(follower.MapToUserContactsViewModel());
     // return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("follow")]
    public async Task<IActionResult> Follow([FromBody]FollowViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter is null");

      if (await _mainDb.Followers.AddFollow(vm.FollowerId, vm.FollowToId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("unfollow")]
    public async Task<IActionResult> Unfollow([FromBody]FollowViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter is null");

      if (await _mainDb.Followers.DeleteFollow(vm.FollowerId, vm.FollowToId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

  }
}
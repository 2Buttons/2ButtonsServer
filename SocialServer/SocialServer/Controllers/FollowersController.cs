﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SocialData;
using SocialServer.ViewModels.InputParameters.ControllersViewModels;
using SocialServer.ViewModels.OutputParameters;

namespace SocialServer.Controllers
{
  [Produces("application/json")]
  [EnableCors("AllowAllOrigin")]
  //[Route("api/[controller]")]
  public class FollowersController : Controller //Represent user's followers or people who user are follow to
  {

    private readonly SocialDataUnitOfWork _socialDb;
    public FollowersController(SocialDataUnitOfWork mainDb)
    {
      _socialDb = mainDb;
    }

    [HttpPost("getFollowers")]
    public async Task<IActionResult> GetFollowers([FromBody]FollowerViewModel vm)
    {
      if (vm?.PageParams == null)
        return BadRequest($"Input parameter {nameof(vm)} is null");
      var followers = await _socialDb.Followers.GetFollowers(vm.UserId, vm.UserPageId, vm.PageParams.Offset,
        vm.PageParams.Count, vm.SearchedLogin);
        return Ok(followers.MapToUserContactsViewModel());
     // return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("getFollowTo")]
    public async Task<IActionResult> GetFollowTo([FromBody]FollowerViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter {nameof(vm)} is null");

      var follower = await _socialDb.Followers.GetFollowTo(vm.UserId, vm.UserPageId, vm.PageParams.Offset, vm.PageParams.Count, vm.SearchedLogin);
        return Ok(follower.MapToUserContactsViewModel());
     // return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("follow")]
    public async Task<IActionResult> Follow([FromBody]FollowViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter is null");

      if (await _socialDb.Followers.AddFollow(vm.FollowerId, vm.FollowToId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("unfollow")]
    public async Task<IActionResult> Unfollow([FromBody]FollowViewModel vm)
    {
      if (vm == null)
        return BadRequest($"Input parameter is null");

      if (await _socialDb.Followers.DeleteFollow(vm.FollowerId, vm.FollowToId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

  }
}
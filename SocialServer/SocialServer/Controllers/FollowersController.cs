using System;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SocialData;
using SocialServer.ViewModels.InputParameters.ControllersViewModels;
using SocialServer.ViewModels.OutputParameters;

namespace SocialServer.Controllers
{
  [Produces("application/json")]
  [EnableCors("AllowAllOrigin")]
  [Route("social/followers")]
  public class FollowersController : Controller //Represent user's followers or people who user are follow to
  {
    private readonly ConnectionsHub _hub;

    private readonly SocialDataUnitOfWork _socialDb;

    public FollowersController(SocialDataUnitOfWork mainDb, ConnectionsHub hub)
    {
      _socialDb = mainDb;
      _hub = hub;
    }

    [HttpGet("server")]
    public IActionResult ServerName()
    {
      return new OkResponseResult("Social Server");
    }

    [HttpPost]
    public async Task<IActionResult> GetFollowers([FromBody] FollowerViewModel vm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var followers = await _socialDb.Followers.GetFollowers(vm.UserId, vm.UserPageId, vm.PageParams.Offset,
        vm.PageParams.Count, vm.SearchedLogin);
      return new OkResponseResult(followers.MapToUserContactsViewModel());
      // return new BadResponseResult("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("to")]
    public async Task<IActionResult> GetFollowTo([FromBody] FollowerViewModel vm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var follower = await _socialDb.Followers.GetFollowTo(vm.UserId, vm.UserPageId, vm.PageParams.Offset,
        vm.PageParams.Count, vm.SearchedLogin);
      return new OkResponseResult(follower.MapToUserContactsViewModel());
      // return new BadResponseResult("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("follow")]
    public async Task<IActionResult> Follow([FromBody] FollowViewModel vm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (!await _socialDb.Followers.AddFollow(vm.UserId, vm.FollowToId))
        return new ResponseResult((int) HttpStatusCode.InternalServerError, "We can not connect you to this user.",
          new {IsFollowed = false});
      _hub.Notifications.SendFollowNotification(vm.UserId, vm.FollowToId, DateTime.UtcNow);
      return new OkResponseResult("Now you follow", new {IsFollowed = true});
    }

    [HttpPost("unfollow")]
    public async Task<IActionResult> Unfollow([FromBody] FollowViewModel vm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (await _socialDb.Followers.DeleteFollow(vm.UserId, vm.FollowToId))
        return new OkResponseResult("Now you unfollow", new {IsFollowed = false});
      return new ResponseResult((int) HttpStatusCode.InternalServerError, "We can not disconnect you to this user.",
        new {IsFollowed = true});
    }
  }
}
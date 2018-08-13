using System;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Entities.Main;
using CommonLibraries.MediaFolders;
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
  [Route("social")]
  public class FollowersController : Controller //Represent user's followers or people who user are follow to
  {
    private ConnectionsHub Hub { get; }

    private SocialDataUnitOfWork SocialDb { get; }
    private MediaConverter MediaConverter { get; }

    public FollowersController(SocialDataUnitOfWork mainDb, ConnectionsHub hub, MediaConverter mediaConverter)
    {
      SocialDb = mainDb;
      Hub = hub;
      MediaConverter = mediaConverter;
    }

    [HttpGet("server")]
    public IActionResult ServerName()
    {
      return new OkResponseResult("Social Server");
    }

    [HttpPost("followers")]
    public async Task<IActionResult> GetFollowers([FromBody] FollowerViewModel vm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var followers = await SocialDb.Followers.GetFollowers(vm.UserId, vm.UserPageId, vm.PageParams.Offset,
        vm.PageParams.Count, vm.SearchedLogin);
      return new OkResponseResult(followers.MapToUserContactsViewModel(MediaConverter));
      // return new BadResponseResult("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("followings")]
    public async Task<IActionResult> GetFollowTo([FromBody] FollowerViewModel vm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var follower = await SocialDb.Followers.GetFollowTo(vm.UserId, vm.UserPageId, vm.PageParams.Offset,
        vm.PageParams.Count, vm.SearchedLogin);
      return new OkResponseResult(follower.MapToUserContactsViewModel(MediaConverter));
      // return new BadResponseResult("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("follow")]
    public async Task<IActionResult> Follow([FromBody] FollowViewModel vm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);


      var follower = new FollowingEntity
      {
        FollowedDate = DateTime.UtcNow,
        UserId = vm.UserId,
        FollowingId = vm.FollowingId,
        IsDeleted = false,
        VisitsCount = 1
      };

      if (!await SocialDb.Followers.AddFollow(follower))
        return new ResponseResult((int)HttpStatusCode.InternalServerError, "We can not connect you to this user.",
          new { IsFollowed = false });
      await Hub.Notifications.SendFollowNotification(vm.UserId, vm.FollowingId, DateTime.UtcNow);
      return new OkResponseResult("Now you follow", new { IsFollowed = true });
    }

    [HttpPost("unfollow")]
    public async Task<IActionResult> Unfollow([FromBody] FollowViewModel vm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (await SocialDb.Followers.DeleteFollow(vm.UserId, vm.FollowingId))
        return new OkResponseResult("Now you unfollow", new { IsFollowed = false });
      return new ResponseResult((int)HttpStatusCode.InternalServerError, "We can not disconnect you to this user.",
        new { IsFollowed = true });
    }
  }
}
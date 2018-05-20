using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SocialData;
using SocialData.Main.Entities.Recommended;
using SocialServer.ViewModels.InputParameters;
using SocialServer.ViewModels.OutputParameters;
using SocialServer.ViewModels.OutputParameters.User;


namespace SocialServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  // [Route("/account")]
  //[Route("api/[controller]")]
  public class FriendsController : Controller
  {
    private static readonly HttpClient Client = new HttpClient();

    private readonly VkAuthSettings _vkAuthSettings;
    private readonly FacebookAuthSettings _fbAuthSettings;



    private readonly SocialDataUnitOfWork _socialDb;



    public FriendsController(IOptions<FacebookAuthSettings> fbAuthSettingsAccessor,
      IOptions<VkAuthSettings> vkAuthSettingsAccessor, IOptions<JwtSettings> jwtOptions,
      SocialDataUnitOfWork socialDb)
    {
      _fbAuthSettings = fbAuthSettingsAccessor.Value;
      _vkAuthSettings = vkAuthSettingsAccessor.Value;

      _socialDb = socialDb;
    }

    [HttpPost("getRecommendedUsers")]
    public async Task<IActionResult> GetRecommendedUsers([FromBody] GetRecommendedUsers user)

    {
      var vkUserId = (await _socialDb.Users.GetUserByUserId(user.UserId)).VkId;

      var vkFriendIdsResponse = await Client.GetStringAsync(
        $"https://api.vk.com/method/friends.get?user_id={vkUserId}&count=5000&access_token={_vkAuthSettings.AppAccess}&v=5.74");
      var vkFriendIds = JsonConvert.DeserializeObject<VkFriendIdsResponse>(vkFriendIdsResponse).Response.Items;

      var dkIds = _socialDb.Users.GetUserIdFromVkId(vkFriendIds);
      var socialFriends =
        await _socialDb.RecommendedPeople.GetRecommendedFromUsersId(dkIds.Select(x => x.UserId).ToList());
       // return BadRequest("Something goes wrong with social friends.");

      var partOffset = user.PageParams.Offset / 3;
      var partCount = user.PageParams.Count / 3;

      var followers = await _socialDb.RecommendedPeople.GetRecommendedFromFollowers(user.UserId, partOffset, partCount);
       // return BadRequest("Something goes wrong with followers.");
      var sameFollows = await _socialDb.RecommendedPeople.GetRecommendedFromFollows(user.UserId, partOffset, partCount);
        //return BadRequest("Something goes wrong with follows as you ))) .");

      Parallel.ForEach(followers, x => { x.CommonFollowsTo = (int) (x.CommonFollowsTo * 1.5); });


      var friendsCount = user.PageParams.Count * 60 / 100;
      var followersCount = user.PageParams.Count - friendsCount;

      MakeFollowersAndFollowsTo(followers, sameFollows, out var followersOut, out var followsOut);

      var result = new RecommendedUsers();

      result.SocialNetFrineds = MakeSocialNetFriends(socialFriends, friendsCount);

      result.Followers = followersOut.Take(followersCount / 2).ToList();
      result.CommonFollowsTo = followsOut.Take(friendsCount - followersCount / 2).ToList();

      return Ok(result);
    }

    private List<RecommendedUserViewModel> MakeSocialNetFriends(
      IEnumerable<RecommendedFromUsersIdDb> recommendedStrangers, int count)
    {
      var result = new List<RecommendedUserViewModel>();

      var randomFriends = recommendedStrangers.PickRandom(count);
      for (var i = 0; i < randomFriends.Count; i++)
      {
        var item = randomFriends[i];
        result.Add(new RecommendedUserViewModel
        {
          Position = i,
          UserId = item.UserId,
          Login = item.Login,
          SmallAvatarLink = item.SmallAvatarLink,
          BirthDate = item.BirthDate,
          SexType = item.Sex
        });
      }
      return result;
    }

    private void MakeFollowersAndFollowsTo(IEnumerable<RecommendedFromFollowersDb> recommendedFromFollowers,
      IEnumerable<RecommendedFromFollowsDb> recommendedFromFollows,
      out List<RecommendedUserViewModel> followers, out List<RecommendedUserViewModel> follows)
    {
      var followersLength = recommendedFromFollowers.Count();
      var followsLength = recommendedFromFollows.Count();

      followers = new List<RecommendedUserViewModel>(followersLength);
      follows = new List<RecommendedUserViewModel>(followsLength);

      foreach (var item in recommendedFromFollowers)
        followers.Add(new RecommendedUserViewModel
        {
          Position = 0,
          UserId = item.UserId,
          Login = item.Login,
          SmallAvatarLink = item.SmallAvatarLink,
          BirthDate = item.BirthDate,
          SexType = item.Sex,
          CommonFollowsTo = item.CommonFollowsTo
        });

      foreach (var item in recommendedFromFollows)
        follows.Add(new RecommendedUserViewModel
        {
          Position = 0,
          UserId = item.UserId,
          Login = item.Login,
          SmallAvatarLink = item.SmallAvatarLink,
          BirthDate = item.BirthDate,
          SexType = item.Sex,
          CommonFollowsTo = item.CommonFollowsTo
        });

      var mainList = new List<RecommendedUserViewModel>(followersLength + followsLength);
      mainList.AddRange(followers);
      mainList.AddRange(follows);

      var lo = 0;
      var hi = mainList.Count - 1;
      var mid = followersLength;
      int i = 0, j = followersLength + 1;
      for (var k = lo; k <= mid; k++)
        if (i > mid) mainList[k].Position = j++;
        else if (j > hi) mainList[k].Position = i++;
        else if (mainList[j].CommonFollowsTo > mainList[i].CommonFollowsTo) mainList[k].Position = j++;
        else mainList[k].CommonFollowsTo = i++;
    }


    [HttpPost("inviteFriends")]
    public async Task<IActionResult> InviteFriends([FromBody] UserIdViewModel user)
    {
      var vkUserId = (await _socialDb.Users.GetUserByUserId(user.UserId)).VkId;
      var vkFriendsDataResponse = await Client.GetStringAsync(
        $"https://api.vk.com/method/friends.get?user_id={vkUserId}&count={5000}&fields=photo_100&name_case=nom&access_token={_vkAuthSettings.AppAccess}&v=5.74");
      var vkFriendsData = JsonConvert.DeserializeObject<VkFriendsDataResponse>(vkFriendsDataResponse).Response.Items;
      var result = vkFriendsData.Select(InviteFriendViewModel.ToViewModel).ToList();
      return Ok(result);
    }
  }
}
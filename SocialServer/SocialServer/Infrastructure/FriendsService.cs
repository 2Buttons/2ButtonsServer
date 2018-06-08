using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SocialData;
using SocialData.Main.Entities.Recommended;
using SocialServer.ViewModels.InputParameters;
using SocialServer.ViewModels.OutputParameters;
using SocialServer.ViewModels.OutputParameters.User;

namespace SocialServer.Infrastructure
{
  public class FriendsService : IFriendsService
  {
    private static readonly HttpClient Client = new HttpClient();

    private readonly SocialDataUnitOfWork _socialDb;

    private readonly VkAuthSettings _vkAuthSettings;
    private readonly FacebookAuthSettings _fbAuthSettings;

    public FriendsService(IOptions<FacebookAuthSettings> fbAuthSettingsAccessor,
      IOptions<VkAuthSettings> vkAuthSettingsAccessor, IOptions<JwtSettings> jwtOptions,
      SocialDataUnitOfWork socialDb)
    {
      _fbAuthSettings = fbAuthSettingsAccessor.Value;
      _vkAuthSettings = vkAuthSettingsAccessor.Value;

      _socialDb = socialDb;
    }


    public async Task<RecommendedUsers> GetRecommendedUsers(GetRecommendedUsers user)
    {
      var vkFriends = GetFriendsFromVk(user.UserId);
      var fbFriends = GetFriendsFromFb(user.UserId);

      await Task.WhenAll(vkFriends, fbFriends);
      vkFriends.Result.AddRange(fbFriends.Result);
      var friendsFromNetwroks = vkFriends.Result.Distinct();
      var socialFriends =
        await _socialDb.RecommendedPeople.GetRecommendedFromUsersId(friendsFromNetwroks);
      // return new BadResponseResult("Something goes wrong with social friends.");

      var partOffset = user.PageParams.Offset / 3;
      var partCount = user.PageParams.Count / 3;

      var followers = await _socialDb.RecommendedPeople.GetRecommendedFromFollowers(user.UserId, partOffset, partCount);
      // return new BadResponseResult("Something goes wrong with followers.");
      var sameFollows = await _socialDb.RecommendedPeople.GetRecommendedFromFollows(user.UserId, partOffset, partCount);
      //return new BadResponseResult("Something goes wrong with follows as you ))) .");

      Parallel.ForEach(followers, x => { x.CommonFollowsTo = (int)(x.CommonFollowsTo * 1.5); });


      var friendsCount = user.PageParams.Count * 60 / 100;
      var followersCount = user.PageParams.Count - friendsCount;

      MakeFollowersAndFollowsTo(followers, sameFollows, out var followersOut, out var followsOut);

      var result = new RecommendedUsers();

      result.SocialFriends = MakeSocialNetFriends(socialFriends, friendsCount);

      result.Followers = followersOut.Take(followersCount / 2).ToList();
      result.CommonFollowsTo = followsOut.Take(friendsCount - followersCount / 2).ToList();

      return result;
    }

    private async Task<List<int>> GetFriendsFromVk(int userId)
    {
      var vkUserId = (await _socialDb.Users.GetUserByUserId(userId)).VkId;
      if (vkUserId == 0)
        return new List<int>();

      var vkFriendIdsResponse = await Client.GetStringAsync(
        $"https://api.vk.com/method/friends.get?user_id={vkUserId}&count=5000&access_token={_vkAuthSettings.AppAccess}&v=5.74");
      var vkFriendIds = JsonConvert.DeserializeObject<VkFriendIdsResponse>(vkFriendIdsResponse).Response.Items;

      return await _socialDb.Users.GetUserIdsFromVkIds(vkFriendIds);
    }

    private async Task<List<int>> GetFriendsFromFb(int userId)
    {
      var user = await _socialDb.Users.GetUserByUserId(userId);
      if (user.FacebookId == 0)
        return new List<int>();

      var fbFriendIdsResponse = await Client.GetStringAsync(
        $"https://graph.facebook.com/v3.0/{user.FacebookId}/friends&access_token={user.FacebookToken}");
      var fbFriendIds = JsonConvert.DeserializeObject<FbFriendIdsResponse>(fbFriendIdsResponse).Response.Items;

      return await _socialDb.Users.GetUserIdsFromVkIds(fbFriendIds);
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

  }


}

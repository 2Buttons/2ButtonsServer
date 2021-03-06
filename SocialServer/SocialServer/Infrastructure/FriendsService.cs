﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SocialData;
using SocialData.Main.DTO;
using SocialData.Main.Queries;
using SocialServer.ViewModels.InputParameters;
using SocialServer.ViewModels.OutputParameters;
using SocialServer.ViewModels.OutputParameters.User;

namespace SocialServer.Infrastructure
{
  public class FriendsService : IFriendsService
  {
    private static  HttpClient Client { get; } = new HttpClient();

    private  SocialDataUnitOfWork SocialDb { get; }

    private  VkAuthSettings VkAuthSettings { get; }
    private  FacebookAuthSettings FbAuthSettings { get; }
    private  ILogger<FriendsService> Logger { get; }
    MediaConverter MediaConverter { get; }

    public FriendsService(IOptions<FacebookAuthSettings> fbAuthSettingsAccessor,
      IOptions<VkAuthSettings> vkAuthSettingsAccessor, IOptions<JwtSettings> jwtOptions,
      SocialDataUnitOfWork socialDb, ILogger<FriendsService> logger, MediaConverter mediaConverter)
    {
      FbAuthSettings = fbAuthSettingsAccessor.Value;
      VkAuthSettings = vkAuthSettingsAccessor.Value;
      Logger = logger;
      SocialDb = socialDb;
      MediaConverter = mediaConverter;
    }


    public async Task<RecommendedUsers> GetRecommendedUsers(GetRecommendedUsers user)
    {
      Logger.LogInformation($"{nameof(FriendsService)}.{nameof(GetRecommendedUsers)}.Start");
      var vkFriends = await GetFriendsFromVk(user.UserId);
     // var fbFriends = GetFriendsFromFb(user.UserId);

      //await Task.WhenAll(vkFriends, fbFriends);
      //vkFriends.AddRange();
      var friendsFromNetwroks = vkFriends.Distinct();
      var socialFriends =
        await SocialDb.RecommendedPeople.GetRecommendedFromUsersId(friendsFromNetwroks);
      // return new BadResponseResult("Something goes wrong with social friends.");

      var partOffset = user.PageParams.Offset ;
      var partCount = user.PageParams.Count;

      var followers = await SocialDb.RecommendedPeople.GetRecommendedFromFollowers(user.UserId, partOffset, partCount);
      // return new BadResponseResult("Something goes wrong with followers.");
      var followings = await SocialDb.RecommendedPeople.GetRecommendedFromFollowings(user.UserId, partOffset, partCount);
      //return new BadResponseResult("Something goes wrong with follows as you ))) .");

      //Parallel.ForEach(followers, x => { x.CommonFollowingsCount = (int)(x.CommonFollowingsCount * 1.5); });

      var recommended = new List<(RecommendedType Type, RecommendedFollowingQuery list)>(followers.Count + followings.Count);

      recommended.AddRange(followers.Select(x=>(RecommendedType.Follower, x)));
      recommended.AddRange(followings.Select(x => (RecommendedType.Following, x)));

      recommended = recommended.OrderByDescending(x =>
      {
        if(x.Type == RecommendedType.Follower)
          return x.list.CommonFollowingsCount*1.5;
        return x.list.CommonFollowingsCount;
      }).Skip(user.PageParams.Offset).Take(user.PageParams.Count).ToList();

      MakeFollowersAndFollowsTo(recommended, out var followersOut, out var followingsOut);


    
      var result = new RecommendedUsers
      {
        SocialFriends = MakeSocialNetFriends(socialFriends, 100),

        Followers = followersOut,
        Followings = followingsOut
      };
      Logger.LogInformation($"{nameof(FriendsService)}.{nameof(GetRecommendedUsers)}.End");
      return result;
    }

    private async Task<List<int>> GetFriendsFromVk(int userId)
    {
      Logger.LogInformation($"{nameof(FriendsService)}.{nameof(GetFriendsFromVk)}.Start");
      var user = await SocialDb.Users.FindUserByUserId(userId);
      if(user == null || user.VkId == 0) return new List<int>();

      var vkFriendIdsResponse = await Client.GetStringAsync(
        $"https://api.vk.com/method/friends.get?user_id={user.VkId}&count=5000&access_token={VkAuthSettings.AppAccess}&v=5.74");
      var vkFriendIds = JsonConvert.DeserializeObject<VkFriendIdsResponse>(vkFriendIdsResponse).Response.Items;

      var result =  await SocialDb.Users.GetUserIdsFromVkIds(vkFriendIds);
      Logger.LogInformation($"{nameof(FriendsService)}.{nameof(GetFriendsFromVk)}.End");
      return result;
    }

    private async Task<List<int>> GetFriendsFromFb(int userId)
    {
      var user = await SocialDb.Users.FindUserByUserId(userId);
      //if (user.FacebookId == 0)
        return new List<int>();

      //var fbFriendIdsResponse = await Client.GetStringAsync(
      //  $"https://graph.facebook.com/v3.0/{user.FacebookId}/friends&access_token={user.FacebookToken}");
      //var fbFriendIds = JsonConvert.DeserializeObject<FbFriendIdsResponse>(fbFriendIdsResponse).Response.Items;

      //return await _socialDb.Users.GetUserIdsFromVkIds(fbFriendIds);
    }

    private List<RecommendedUserViewModel> MakeSocialNetFriends(
      IEnumerable<RecommendedFromUsersIdDto> recommendedStrangers, int count)
    {
      Logger.LogInformation($"{nameof(FriendsService)}.{nameof(MakeSocialNetFriends)}.Start");
      var result = new List<RecommendedUserViewModel>();

      var randomFriends = recommendedStrangers.PickRandom(count);
      for (var i = 0; i < randomFriends.Count; i++)
      {
        var item = randomFriends[i];
        result.Add(new RecommendedUserViewModel
        {
          Position = i,
          UserId = item.UserId,
          Login = item.FirstName + " " + item.LastName,
          SmallAvatarUrl = MediaConverter.ToFullAvatarUrl(item.OriginalAvatarUrl, AvatarSizeType.Small),
          Age = item.BirthDate.Age(),
          SexType = item.SexType
        });
      }
      Logger.LogInformation($"{nameof(FriendsService)}.{nameof(MakeSocialNetFriends)}.End");
      return result;
    }

    private void MakeFollowersAndFollowsTo(IEnumerable<(RecommendedType Type, RecommendedFollowingQuery User)> recommended,
      out List<RecommendedUserViewModel> followers, out List<RecommendedUserViewModel> followings)
    {
      Logger.LogInformation($"{nameof(FriendsService)}.{nameof(MakeFollowersAndFollowsTo)}.Start");

      followers = new List<RecommendedUserViewModel>();
      followings = new List<RecommendedUserViewModel>();

      var valueTuples = recommended as List<(RecommendedType Type, RecommendedFollowingQuery User)> ?? recommended.ToList();
      for (int i = 0; i < valueTuples.Count; i++)
      {
        var item = valueTuples.ElementAt(i);
        var user = new RecommendedUserViewModel
        {
          Position = i,
          UserId = item.User.UserId,
          Login = item.User.FirstName + " " + item.User.LastName,
          SmallAvatarUrl = MediaConverter.ToFullAvatarUrl(item.User.OriginalAvatarUrl, AvatarSizeType.Small),
          Age = item.User.BirthDate.Age(),
          SexType = item.User.SexType,
          CommonFollowingsCount = item.User.CommonFollowingsCount
        };

        switch (item.Type)
        {
          case RecommendedType.Follower:
            followers.Add(user);
            break;
          case RecommendedType.Following:
            followings.Add(user);
            break;
        }
      }
      Logger.LogInformation($"{nameof(FriendsService)}.{nameof(MakeFollowersAndFollowsTo)}.End");
    }

  }

  public enum RecommendedType
  {
    Follower = 0,
    Following = 1
  }


}

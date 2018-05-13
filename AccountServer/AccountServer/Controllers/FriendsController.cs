﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountServer.Auth;
using AccountServer.Models;
using AccountServer.Models.Facebook;
using AccountServer.Models.Vk;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.OutputParameters;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TwoButtonsAccountDatabase;
using TwoButtonsAccountDatabase.DTO;
using TwoButtonsAccountDatabase.Entities;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;


namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  // [Route("/account")]
  public class FriendsController : Controller
  {
    //some config in the appsettings.json
    private readonly JwtIssuerOptions _jwtOptions;

    private readonly IJwtFactory _jwtFactory;
    //repository to handler the sqlite database

    private readonly TwoButtonsContext _dbMain;
    private readonly AccountUnitOfWork _accountDb;
    private readonly FacebookAuthSettings _fbAuthSettings;
    private readonly VkAuthSettings _vkAuthSettings;
    private static readonly HttpClient Client = new HttpClient();

    public FriendsController(IOptions<FacebookAuthSettings> fbAuthSettingsAccessor, IOptions<VkAuthSettings> vkAuthSettingsAccessor, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, TwoButtonsContext buttonsContext, AccountUnitOfWork accountDb)
    {
      _fbAuthSettings = fbAuthSettingsAccessor.Value;
      _vkAuthSettings = vkAuthSettingsAccessor.Value;
      _jwtFactory = jwtFactory;
      _jwtOptions = jwtOptions.Value;
      _accountDb = accountDb;
      _dbMain = buttonsContext;
    }

    [HttpPost("getRecommendedUsers")]
    public async Task<IActionResult> GetRecommendedUsers([FromBody] UserIdViewModel user)

    {
      var vkUserId = (await _accountDb.Users.GetUserByUserId(user.UserId)).VkId;

      var vkFriendIdsResponse = await Client.GetStringAsync($"https://api.vk.com/method/friends.get?user_id={vkUserId}&count=5000&access_token={_vkAuthSettings.AppAccess}&v=5.74");
      var vkFriendIds = JsonConvert.DeserializeObject<VkFriendIdsResponse>(vkFriendIdsResponse).Response.Items;

      var dkIds = _accountDb.Users.GetUserIdFromVkId(vkFriendIds);
      if (!RecommendedSubscribersWrapper.TryGetRecommendedFromUsersId(_dbMain, dkIds.Select(x => x.UserId).ToList(),
        out var socialFriends))
        return BadRequest("Something goes wrong.");
      if (!RecommendedSubscribersWrapper.TryGetRecommendedFromFollowers(_dbMain, user.UserId,
          out var followers))
          return BadRequest("Something goes wrong.");
      if (!RecommendedSubscribersWrapper.TryGetRecommendedFromFollows(_dbMain, user.UserId,
        out var sameFollows))
        return BadRequest("Something goes wrong.");

      List<RecommendedUserViewModel> result = new List<RecommendedUserViewModel>();
      foreach (var item in socialFriends)
      {
        result.Add(new RecommendedUserViewModel
        {
          UserId = item.UserId,
          Login = item.Login,
          SmallAvatarLink = item.SmallAvatarLink,
          SexType = item.Sex
        });
      }

      foreach (var item in followers)
      {
        result.Add(new RecommendedUserViewModel
        {
          UserId = item.UserId,
          Login = item.Login,
          SmallAvatarLink = item.SmallAvatarLink,
          SexType = item.Sex
        });
      }

      foreach (var item in sameFollows)
      {
        result.Add(new RecommendedUserViewModel
        {
          UserId = item.UserId,
          Login = item.Login,
          SmallAvatarLink = item.SmallAvatarLink,
          SexType = item.Sex
        });
      }

      return Ok(result);
    }


    [HttpPost("inviteFriends")]
    public async Task<IActionResult> InviteFriends([FromBody] UserIdViewModel user)
    {
      var vkUserId =(await _accountDb.Users.GetUserByUserId(user.UserId)).VkId;
      var vkFriendsDataResponse = await Client.GetStringAsync(
        $"https://api.vk.com/method/friends.get?user_id={vkUserId}&count={5000}&fields=photo_100&name_case=nom&access_token={_vkAuthSettings.AppAccess}&v=5.74");
      var vkFriendsData = JsonConvert.DeserializeObject<VkFriendsDataResponse>(vkFriendsDataResponse).Response.Items;
      var result = vkFriendsData.Select(InviteFriendViewModel.ToViewModel).ToList();
      return Ok(result);
    }
  }
}
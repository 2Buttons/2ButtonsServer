﻿using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountData;
using AccountData.DTO;
using AccountServer.Infrastructure.Services;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  // [Route("/account")]
  public class AccountController : Controller
  {
    private readonly IAccountService _account;
    private NotificationsMessageHandler _notificationsMessageHandler;

    public AccountController(IAccountService accountService, NotificationsMessageHandler notificationsMessageHandler)
    {
      _account = accountService;
      _notificationsMessageHandler = notificationsMessageHandler;
    }

    [HttpGet("server")]
    public IActionResult ServerName()
    {
      _notificationsMessageHandler.SendMessageToAllAsync(JsonConvert.SerializeObject(new ResponseObject(200,"Account","Hi")));
      return new OkResponseResult((object)"Account");
    }

    [Authorize]
    [HttpPost("{userPageId:int}")]
    public async Task<IActionResult> GetUser(int userPageId)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      var result = await _account.GetUserAsync(userId, userPageId);
      return new OkResponseResult(result);
    }

    [Authorize]
    [HttpPost("getAuth")]
    public async Task<IActionResult> GetUserInfoAuth([FromBody] UserIdViewModel userPage)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      var result = await _account.GetUserAsync(userId, userPage.UserId);

      return new OkResponseResult(result);
    }

    [HttpPost("update")]
    public async Task<IActionResult> GetUserInfoAuth([FromBody] UpdateUserInfoDto user)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      if (await _account.UpdateUserInfoAsync(user))
        return new OkResponseResult();
      return new ResponseResult((int)HttpStatusCode.NotModified);
    }


    [HttpPost("addSocial")]
    public async Task<IActionResult> AddSocial ([FromBody] AddSocialViewModel socialAuth)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (socialAuth.State != "123456")
      {
        ModelState.AddModelError("State", "You are hacker! Your state in incorret");
        return new BadResponseResult(ModelState);
      }
      if (!socialAuth.Status || !socialAuth.Error.IsNullOrEmpty())
      {
        ModelState.AddModelError("ExternalError", "ExternalError: " + socialAuth.Error + " " + socialAuth.ErrorDescription);
        return new BadResponseResult(ModelState);
      }
      if (string.IsNullOrEmpty(socialAuth.Code))
      {
        ModelState.AddModelError("Code", "Code is null or empty.");
        return new BadResponseResult(ModelState);
      }

      if (await _account.AddUserSocialAsync(socialAuth.UserId, socialAuth.Code,socialAuth.SocialType))
        return new OkResponseResult();
      return new ResponseResult((int)HttpStatusCode.NotModified);
    }


    [HttpPost("get")]
    public async Task<IActionResult> GetUserInfo([FromBody] UserPageIdViewModel userPage)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      var result = await _account.GetUserAsync(userPage.UserId, userPage.UserPageId);

      return new OkResponseResult(result);
    }
  }
}
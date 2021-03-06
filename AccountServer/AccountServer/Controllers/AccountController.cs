﻿using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountData.DTO;
using AccountServer.Infrastructure.Services;
using AccountServer.ViewModels.InputParameters;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("account")]
  public class AccountController : Controller
  {
    private readonly IAccountService _account;

    public AccountController(IAccountService accountService)
    {
      _account = accountService;
    }

    [HttpPost("server")]
    public IActionResult ServerName()
    {
      return new OkResponseResult((object)"Account");
    }

    [HttpPost("getCityAndBirthdate")]
    public async Task<IActionResult> GetCityAndBirthdate([FromBody] UserIdViewModel userPage)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      var (city, birthdate) = await _account.GetCityAndBirthdate(userPage.UserId);

      return new OkResponseResult(new { UserId = userPage.UserId, City = city, BirthDate = birthdate });
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
    public async Task<IActionResult> GetUserInfoAuth([FromBody] UserPageIdViewModel userPage)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "0");
      var result = await _account.GetUserAsync(userId, userPage.UserPageId);

      return new OkResponseResult(result);
    }

    [HttpPost("update")]
    public async Task<IActionResult> GetUserInfoAuth([FromBody] UpdateUserInfoViewModel user)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      var updateUser = new UpdateUserInfoDto
      {
        BirthDate = user.BirthDate,
        City = user.City,
        Description = user.Description,
        SexType = user.SexType,
        UserId = user.UserId
      };

      var (firstName, lastName) = _account.ParseLoginIntoFirstNameAndLastName(user.Login);
      updateUser.FristName = firstName;
      updateUser.LastName = lastName;

      if (await _account.UpdateUserInfoAsync(updateUser))
        return new OkResponseResult();
      return new ResponseResult((int)HttpStatusCode.NotModified);
    }


    [HttpPost("addSocial")]
    public async Task<IActionResult> AddSocial([FromBody] AddSocialViewModel socialAuth)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      if (socialAuth.State != "S5ocialCode!129_Code")
      {
        ModelState.AddModelError("State", "You are hacker! Your state in incorrect");
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

      if (await _account.AddUserSocialAsync(socialAuth.UserId, socialAuth.Code, socialAuth.SocialType))
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
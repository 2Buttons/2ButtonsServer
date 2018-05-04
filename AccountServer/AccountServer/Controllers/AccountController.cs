﻿using System.Security.Claims;
using AccountServer.Entities;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
 // [Route("/account")]
  public class AccountController : Controller
  {
    private readonly TwoButtonsContext _twoButtonsContext;

    public AccountController(TwoButtonsContext context)
    {
      _twoButtonsContext = context;
    }

    [HttpGet("register")]
    public IActionResult AddUser()
    {
      return BadRequest("Please, use POST request.");
    }

    [HttpPost("register")]
    public IActionResult RegisterUser([FromBody] UserRegistrationViewModel user)
    {
      if (user == null)
        return BadRequest($"Input parameter  is null");
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var role = RoleType.User;

      if (UserWrapper.TryAddUser(_twoButtonsContext, user.Login, user.Password, user.Age, (int) user.SexType,
        user.City, user.Phone, user.Description, user.FullAvatarLink, user.SmallAvatarLink, (int)role, out var userId))
        return Ok(userId);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }


    [HttpPost("checkLogin")]
    public IActionResult CheckValidLogin([FromBody] LoginViewModel login)
    {
      if (string.IsNullOrEmpty(login?.Login))
        return BadRequest($"Input parameter  is null");
      if (!LoginWrapper.TryCheckValidLogin(_twoButtonsContext, login.Login, out var isValid))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");
      return Ok(isValid);
    }

    [Authorize]
    [HttpPost("getUserInfoAuth")]
    public IActionResult GetUserInfoAuth([FromBody]UserPageIdViewModel userPage)
    {
      if (userPage == null)
        return BadRequest($"Input parameter  is null");

      var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);

      if (!UserWrapper.TryGetUserInfo(_twoButtonsContext, userId, userPage.UserPageId, out var userInfo))
        return BadRequest("Something goes wrong in TryGetUserInfo. We will fix it!... maybe)))");
      if (!UserWrapper.TryGetUserStatistics(_twoButtonsContext, userId, out var userStatistics))
        return BadRequest("Something goes wrong in TryGetUserStatistics. We will fix it!... maybe)))");
      if (!UserWrapper.TryGetUserContacts(_twoButtonsContext, userId, out var userContacts))
        return BadRequest("Something goes wrong in TryGetUserContacts. We will fix it!... maybe)))");


      var result = userInfo.MapToUserInfoViewModel(userStatistics, userContacts);

      return Ok(result);
    }

    [HttpPost("getUserInfo")]
    public IActionResult GetUserInfo([FromBody]UserPageIdViewModel userPage)
    {
      if (userPage == null)
        return BadRequest($"Input parameter  is null");

      //var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);

      if (!UserWrapper.TryGetUserInfo(_twoButtonsContext, userPage.UserId, userPage.UserPageId, out var userInfo))
        return BadRequest("Something goes wrong in TryGetUserInfo. We will fix it!... maybe)))");
      if (!UserWrapper.TryGetUserStatistics(_twoButtonsContext, userPage.UserId, out var userStatistics))
        return BadRequest("Something goes wrong in TryGetUserStatistics. We will fix it!... maybe)))");
      if (!UserWrapper.TryGetUserContacts(_twoButtonsContext, userPage.UserId, out var userContacts))
        return BadRequest("Something goes wrong in TryGetUserContacts. We will fix it!... maybe)))");


      var result = userInfo.MapToUserInfoViewModel(userStatistics, userContacts);

      return Ok(result);
    }

  }
}
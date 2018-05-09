using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountServer.Auth;
using AccountServer.Data;
using AccountServer.Helpers;
using AccountServer.Models;
using AccountServer.Models.Facebook;
using AccountServer.SocialNets;
using AccountServer.ViewModels;
using AccountServer.ViewModels.InputParameters;
using AccountServer.ViewModels.InputParameters.Auth;
using AccountServer.ViewModels.OutputParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TwoButtonsDatabase;
using TwoButtonsDatabase.WrapperFunctions;

namespace AccountServer.Controllers
{
[EnableCors("AllowAllOrigin")]

  public class ExternalAuthController : Controller
  {
    //some config in the appsettings.json
    private JwtIssuerOptions _jwtOptions;

    private IJwtFactory _jwtFactory;
    //repository to handler the sqlite database


    private IMemoryCache _cache;

    private TwoButtonsContext _dbMain;
    private AuthenticationRepository _dbToken;
  //  private static readonly HttpClient Client = new HttpClient();



    private readonly FacebookAuthSettings _fbAuthSettings;
    //private readonly IJwtFactory _jwtFactory;
    //private readonly JwtIssuerOptions _jwtOptions;
    private static readonly HttpClient Client = new HttpClient();

    public ExternalAuthController(IOptions<FacebookAuthSettings> fbAuthSettingsAccessor,  IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
    {
      _fbAuthSettings = fbAuthSettingsAccessor.Value;
      //_userManager = userManager;
      //_appDbContext = appDbContext;
      _jwtFactory = jwtFactory;
      _jwtOptions = jwtOptions.Value;
    }

  

    [HttpPost("vk/code")]
    public async Task<IActionResult> VkCode([FromBody]VkAuthViewModel model)
    {
      var z = await Client.GetStringAsync($"https://oauth.vk.com/access_token?client_id=6469856&client_secret=Zcec8RyHjpvaVfvRxLvq&redirect_uri=http://localhost:6256/vk-auth-code.html&code={model.Code}");
      model.AccessToken = JsonConvert.DeserializeObject<VkAppAccessTokenCode>(z).AccessToken;
      model.UserId = JsonConvert.DeserializeObject<VkAppAccessTokenCode>(z).UserId;
      var vk = new VkMethods(model.AccessToken);
      //var info = await vk.GetAccountInfo();
      
      var friends = await vk.GetFriends(model.UserId);
      return Ok();
      // 1.generate an app access token
      var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
      var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
      // 2. validate the user access token
      var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
      var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

      if (!userAccessTokenValidation.Data.IsValid)
      {
        return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid facebook token.", ModelState));
      }

      // 3. we've got a valid token so we can request user data from fb
      var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
      var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

      var p = 5;
      // 4. ready to create the local user account (if necessary) and jwt
      // var user = await _userManager.FindByEmailAsync(userInfo.Email);

      //if (user == null)
      //{
      //  var appUser = new AppUser
      //  {
      //    FirstName = userInfo.FirstName,
      //    LastName = userInfo.LastName,
      //    FacebookId = userInfo.Id,
      //    Email = userInfo.Email,
      //    UserName = userInfo.Email,
      //    PictureUrl = userInfo.Picture.Data.Url
      //  };

      //  var result = await _userManager.CreateAsync(appUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

      //  if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

      //  await _appDbContext.Customers.AddAsync(new Customer { IdentityId = appUser.Id, Location = "", Locale = userInfo.Locale, Gender = userInfo.Gender });
      //  await _appDbContext.SaveChangesAsync();
      //}

      //// generate the jwt for the local user...
      //var localUser = await _userManager.FindByNameAsync(userInfo.Email);

      //if (localUser == null)
      //{
      //  return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.", ModelState));
      //}

      //var jwt = await Tokens.GenerateJwt(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id), _jwtFactory, localUser.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

      return new OkObjectResult("Good");
    }

    [HttpPost("vk")]
    public async Task<IActionResult> Vk([FromBody]VkAuthViewModel model)
    {

      var vk = new VkMethods(model.AccessToken);
   //   var info = await vk.GetAccountInfo();

      var friends = await vk.GetFriends(model.UserId);

      return Ok();
      // 1.generate an app access token
      var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
      var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
      // 2. validate the user access token
      var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
      var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

      if (!userAccessTokenValidation.Data.IsValid)
      {
        return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid facebook token.", ModelState));
      }

      // 3. we've got a valid token so we can request user data from fb
      var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
      var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

      var p = 5;
      // 4. ready to create the local user account (if necessary) and jwt
      // var user = await _userManager.FindByEmailAsync(userInfo.Email);

      //if (user == null)
      //{
      //  var appUser = new AppUser
      //  {
      //    FirstName = userInfo.FirstName,
      //    LastName = userInfo.LastName,
      //    FacebookId = userInfo.Id,
      //    Email = userInfo.Email,
      //    UserName = userInfo.Email,
      //    PictureUrl = userInfo.Picture.Data.Url
      //  };

      //  var result = await _userManager.CreateAsync(appUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

      //  if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

      //  await _appDbContext.Customers.AddAsync(new Customer { IdentityId = appUser.Id, Location = "", Locale = userInfo.Locale, Gender = userInfo.Gender });
      //  await _appDbContext.SaveChangesAsync();
      //}

      //// generate the jwt for the local user...
      //var localUser = await _userManager.FindByNameAsync(userInfo.Email);

      //if (localUser == null)
      //{
      //  return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.", ModelState));
      //}

      //var jwt = await Tokens.GenerateJwt(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id), _jwtFactory, localUser.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

      return new OkObjectResult("Good");
    }

    [HttpPost("facebook")]
    public async Task<IActionResult> Facebook([FromBody]FacebookAuthViewModel model)
    {
      // 1.generate an app access token
      var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
      var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
      // 2. validate the user access token
      var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
      var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

      if (!userAccessTokenValidation.Data.IsValid)
      {
        return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid facebook token.", ModelState));
      }

      // 3. we've got a valid token so we can request user data from fb
      var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
      var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

      var p = 5;
      // 4. ready to create the local user account (if necessary) and jwt
     // var user = await _userManager.FindByEmailAsync(userInfo.Email);

      //if (user == null)
      //{
      //  var appUser = new AppUser
      //  {
      //    FirstName = userInfo.FirstName,
      //    LastName = userInfo.LastName,
      //    FacebookId = userInfo.Id,
      //    Email = userInfo.Email,
      //    UserName = userInfo.Email,
      //    PictureUrl = userInfo.Picture.Data.Url
      //  };

      //  var result = await _userManager.CreateAsync(appUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

      //  if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

      //  await _appDbContext.Customers.AddAsync(new Customer { IdentityId = appUser.Id, Location = "", Locale = userInfo.Locale, Gender = userInfo.Gender });
      //  await _appDbContext.SaveChangesAsync();
      //}

      //// generate the jwt for the local user...
      //var localUser = await _userManager.FindByNameAsync(userInfo.Email);

      //if (localUser == null)
      //{
      //  return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.", ModelState));
      //}

      //var jwt = await Tokens.GenerateJwt(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id), _jwtFactory, localUser.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

      return new OkObjectResult("Good");
    }
  }
}


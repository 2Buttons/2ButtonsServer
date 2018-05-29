using System.Collections.Generic;
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
using CommonLibraries.Helpers;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("/avatar")]
  public class AvatarController : Controller
  {
    private readonly IAccountService _account;

    public AvatarController(IAccountService accountService)
    {
      _account = accountService;
    }

    [HttpGet("update/file")]
    public async Task<IActionResult> UpdateAvatarViaLink(UpdateAvatarFileViewModel avatar)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      //var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      if (!await _account.UpdateAvatarViaFile(avatar.UserId, avatar.Size, avatar.File))
        return new ResponseResult((int)HttpStatusCode.NotModified);
      return new OkResponseResult();
    }

    [HttpGet("update/link")]
    public async Task<IActionResult> UpdateAvatarViaFile(UpdateAvatarUrlViewModel avatar)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      //var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      if (!await _account.UpdateAvatarViaLink(avatar.UserId, avatar.Size, avatar.Url))
        return new ResponseResult((int)HttpStatusCode.NotModified);
      return new OkResponseResult();
    }

  }
}
using System.Net;
using System.Threading.Tasks;
using AccountServer.Infrastructure.Services;
using AccountServer.ViewModels.InputParameters;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("account/avatar")]
  public class AvatarController : Controller
  {
    private readonly IAccountService _account;

    public AvatarController(IAccountService accountService)
    {
      _account = accountService;
    }

    [HttpPost("update/file")]
    public async Task<IActionResult> UpdateAvatarViaUrl([FromBody]UpdateAvatarFileViewModel avatar)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      //var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      var (isUpdated, url) = await _account.UpdateAvatarViaFile(avatar.UserId, avatar.Size, avatar.File);
      return !isUpdated ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult("Avatar was updated", new { Url = url });
    }

    [HttpPost("update/link")]
    public async Task<IActionResult> UpdateAvatarViaFile([FromBody]UpdateAvatarUrlViewModel avatar)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      //var userId = int.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
      var (isUpdated, url) = await _account.UpdateAvatarViaUrl(avatar.UserId, avatar.Size, avatar.Url);
      return !isUpdated ? new ResponseResult((int)HttpStatusCode.NotModified) : new OkResponseResult("Avatar was updated", new { Url = url });
    }

  }
}
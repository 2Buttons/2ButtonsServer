using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using CommonLibraries.SocialNetworks.Facebook;
using CommonLibraries.SocialNetworks.Vk;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SocialData;
using SocialData.Main.Entities.Recommended;
using SocialServer.Infrastructure;
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
    private readonly IFriendsService _friendsService;

    public FriendsController(IFriendsService friendsService)
    {
      _friendsService = friendsService;
    }

    [HttpPost("getRecommendedUsers")]
    public async Task<IActionResult> GetRecommendedUsers([FromBody] GetRecommendedUsers user)

    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var result = _friendsService.GetRecommendedUsers(user);

      return new OkResponseResult(result);
    }

   

    [HttpPost("inviteFriends")]
    public async Task<IActionResult> InviteFriends([FromBody] UserIdViewModel user)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      //var vkUserId = (await _socialDb.Users.GetUserByUserId(user.UserId)).VkId;
      //var vkFriendsDataResponse = await Client.GetStringAsync(
      //  $"https://api.vk.com/method/friends.get?user_id={vkUserId}&count={5000}&fields=photo_100&name_case=nom&access_token={_vkAuthSettings.AppAccess}&v=5.74");
      //var vkFriendsData = JsonConvert.DeserializeObject<VkFriendsDataResponse>(vkFriendsDataResponse).Response.Items;
      //var result = vkFriendsData.Select(InviteFriendViewModel.ToViewModel).ToList();
      return new OkResponseResult();
    }
  }
}
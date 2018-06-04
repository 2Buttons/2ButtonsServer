using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountData;
using AccountData.DTO;
using AccountData.Main.Entities;
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
  [Route("/feedbacks")]
  public class FeedbackController : Controller
  {
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService  feedbackService)
    {
      _feedbackService = feedbackService;
    }

   
    [HttpPost("add")]
    public async Task<IActionResult> GetUser([FromBody]FeedbackViewModel feedback)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      FeedbackDb feedbackDb = new FeedbackDb {UserId = feedback.UserId, Type = feedback.Type, Text = feedback.Text};
      if (!await _feedbackService.AddFeedback(feedbackDb))
        return new ResponseResult((int) HttpStatusCode.InternalServerError, "We can not add your feedback.");
      return new OkResponseResult("We add your feedback.");
    }

    [Authorize]
    [HttpPost("addAuth")]
    public async Task<IActionResult> GetUserInfoAuth([FromBody] FeedbackViewModel feedback)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      FeedbackDb feedbackDb = new FeedbackDb { UserId = feedback.UserId, Type = feedback.Type, Text = feedback.Text };
      if (!await _feedbackService.AddFeedback(feedbackDb))
        return new ResponseResult((int)HttpStatusCode.InternalServerError, "We can not add your feedback.");
      return new OkResponseResult("We add your feedback.");
    }


    [HttpPost]
    public async Task<IActionResult> GetUserFeedbacks([FromBody] UserIdViewModel userPage)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      return new OkResponseResult(await _feedbackService.GetUserFeedbacks(userPage.UserId));
    }
  }
}
using System;
using System.Net;
using System.Threading.Tasks;
using AccountServer.Infrastructure.Services;
using AccountServer.ViewModels.InputParameters;
using CommonLibraries.Entities.Main;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("account/feedbacks")]
  public class FeedbackController : Controller
  {
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService  feedbackService)
    {
      _feedbackService = feedbackService;
    }

   
    [HttpPost("add")]
    public async Task<IActionResult> AddFeedback([FromBody]FeedbackViewModel feedback)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);

      FeedbackEntity feedbackDb = new FeedbackEntity {UserId = feedback.UserId, FeedbackType = feedback.FeedbackType, Text = feedback.Text, AddedDate = DateTime.UtcNow};
      if (!await _feedbackService.AddFeedback(feedbackDb))
        return new ResponseResult((int) HttpStatusCode.InternalServerError, "We can not add your feedback.");
      return new OkResponseResult("We add your feedback.");
    }

    [Authorize]
    [HttpPost("addAuth")]
    public async Task<IActionResult> AddFeedbackAuth([FromBody] FeedbackViewModel feedback)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      FeedbackEntity feedbackDb = new FeedbackEntity { UserId = feedback.UserId, FeedbackType = feedback.FeedbackType, Text = feedback.Text };
      if (!await _feedbackService.AddFeedback(feedbackDb))
        return new ResponseResult((int)HttpStatusCode.InternalServerError, "We can not add your feedback.");
      return new OkResponseResult("We add your feedback.");
    }

    [HttpPost]
    public async Task<IActionResult> GetFeedbacks([FromBody] PageParams pageParams)
    {
      return new OkResponseResult(await _feedbackService.GetFeedbacks(pageParams.Offset, pageParams.Count));
    }

    [HttpPost("byUser")]
    public async Task<IActionResult> GetUserFeedbacks([FromBody] UserIdViewModel userPage)
    {
      if (!ModelState.IsValid)
        return new BadResponseResult(ModelState);
      return new OkResponseResult(await _feedbackService.GetUserFeedbacks(userPage.UserId));
    }
  }
}
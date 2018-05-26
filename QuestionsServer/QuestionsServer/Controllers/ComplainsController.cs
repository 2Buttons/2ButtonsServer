using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuestionsData;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;

namespace QuestionsServer.Controllers
{
  [Produces("application/json")]
  [EnableCors("AllowAllOrigin")]
  //[Route("complains")]
  public class ComplainsController : Controller
  {
    private readonly QuestionsUnitOfWork _mainDb;

    public ComplainsController(QuestionsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    [HttpPost("addComplain")]
    public async Task<IActionResult> AddComplaint([FromBody] AddComplaintViewModel complaint)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (await _mainDb.Complaints.AddComplaint(complaint.UserId, complaint.QuestionId, complaint.ComplainType))
        return new ResponseResult((int)HttpStatusCode.Created, (object)"Question was deleted.");
      return new ResponseResult((int)HttpStatusCode.NotModified, "Question was not deleted.");
    }

    // только модератору можно
    [HttpPost("getComplains")]
    public async Task<IActionResult> GetComplaints()
    {
      var complaints = await _mainDb.Complaints.GetComplaints();
      return new OkResponseResult(complaints);
    }

    [Authorize(Roles = "moderator, admin")]
    [HttpPost("getComplainsAuth")]
    public async Task<IActionResult> GetComplaintsAuth()
    {
      var complaints = await _mainDb.Complaints.GetComplaints();
      return new OkResponseResult(complaints);
    }
  }
}

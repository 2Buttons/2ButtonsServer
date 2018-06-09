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
  [Route("questions/complaints")]
  public class ComplaintsController : Controller
  {
    private readonly QuestionsUnitOfWork _mainDb;

    public ComplaintsController(QuestionsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddComplaint([FromBody] AddComplaintViewModel complaintt)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      if (await _mainDb.Complaints.AddComplaint(complaintt.UserId, complaintt.QuestionId, complaintt.ComplaintType))
        return new ResponseResult((int)HttpStatusCode.Created, (object)"Question was deleted.");
      return new ResponseResult((int)HttpStatusCode.NotModified, "Question was not deleted.");
    }

    // только модератору можно
    [HttpPost]
    public async Task<IActionResult> GetComplaints()
    {
      var complaintts = await _mainDb.Complaints.GetComplaints();
      return new OkResponseResult(complaintts);
    }

    [Authorize(Roles = "moderator, admin")]
    [HttpPost("auth")]
    public async Task<IActionResult> GetComplaintsAuth()
    {
      var complaintts = await _mainDb.Complaints.GetComplaints();
      return new OkResponseResult(complaintts);
    }
  }
}

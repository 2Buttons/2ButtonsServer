using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<ComplaintsController> _logger;

    public ComplaintsController(QuestionsUnitOfWork mainDb, ILogger<ComplaintsController> logger)
    {
      _mainDb = mainDb;
      _logger = logger;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddComplaint([FromBody] AddComplaintViewModel complaintt)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      _logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(AddComplaint)}.Start");
      var result = await _mainDb.Complaints.AddComplaint(complaintt.UserId, complaintt.QuestionId,
        complaintt.ComplaintType);
      _logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(AddComplaint)}.End");
      return result ? new ResponseResult((int)HttpStatusCode.Created, (object)"Question was deleted.") : new ResponseResult((int)HttpStatusCode.NotModified, "Question was not deleted.");
    }

    // только модератору можно
    [HttpPost]
    public async Task<IActionResult> GetComplaints()
    {
      _logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(GetComplaints)}.Start");
      var complaintts = await _mainDb.Complaints.GetComplaints();
      _logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(GetComplaints)}.End");
      return new OkResponseResult(complaintts);
    }

    [Authorize(Roles = "moderator, admin")]
    [HttpPost("auth")]
    public async Task<IActionResult> GetComplaintsAuth()
    {
      _logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(GetComplaintsAuth)}.Start");
      var complaintts = await _mainDb.Complaints.GetComplaints();
      _logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(GetComplaintsAuth)}.End");
      return new OkResponseResult(complaintts);
    }
  }
}

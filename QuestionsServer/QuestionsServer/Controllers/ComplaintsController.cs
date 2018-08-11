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
    private QuestionsUnitOfWork MainDb { get; }
    private ILogger<ComplaintsController> Logger { get; }

    public ComplaintsController(QuestionsUnitOfWork mainDb, ILogger<ComplaintsController> logger)
    {
      MainDb = mainDb;
      Logger = logger;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddComplaint([FromBody] AddComplaintViewModel complaintt)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      Logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(AddComplaint)}.Start");
      var result = await MainDb.Complaints.AddComplaint(complaintt.UserId, complaintt.QuestionId,
        complaintt.ComplaintType);
      Logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(AddComplaint)}.End");
      return result
        ? new ResponseResult((int) HttpStatusCode.Created, (object) "Question was deleted.")
        : new ResponseResult((int) HttpStatusCode.NotModified, "Question was not deleted.");
    }

    // только модератору можно
    [HttpPost]
    public async Task<IActionResult> GetComplaints()
    {
      Logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(GetComplaints)}.Start");
      var complaintts = await MainDb.Complaints.GetComplaints();
      Logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(GetComplaints)}.End");
      return new OkResponseResult(complaintts);
    }

    [Authorize(Roles = "moderator, admin")]
    [HttpPost("auth")]
    public async Task<IActionResult> GetComplaintsAuth()
    {
      Logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(GetComplaintsAuth)}.Start");
      var complaintts = await MainDb.Complaints.GetComplaints();
      Logger.LogInformation($"{nameof(ComplaintsController)}.{nameof(GetComplaintsAuth)}.End");
      return new OkResponseResult(complaintts);
    }
  }
}
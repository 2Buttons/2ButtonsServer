using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.Moderators;
using TwoButtonsDatabase.Repositories;
using TwoButtonsServer.ViewModels;
using TwoButtonsServer.ViewModels.InputParameters;
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;
using TwoButtonsServer.ViewModels.OutputParameters;

namespace TwoButtonsServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("api/[controller]")]
  public class QuestionsController : Controller //Controller for a Question
  {
    private readonly TwoButtonsContext _context;
    public QuestionsController(TwoButtonsContext context)
    {
      _context = context;
    }

    [HttpPost("getQuestion")]
    public IActionResult GetQuestion([FromBody]QuestionIdViewModel inputQuestion)
    {
      if (inputQuestion == null)
        return BadRequest($"Input parameter  is null");

      if (!QuestionRepository.TryGetQuestion(_context, inputQuestion.UserId, inputQuestion.QuestionId, out var question))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");


      var commentsAmount = 10000;
      var photosAmount = 100;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      if (!TagsRepository.TryGetTags(_context, question.QuestionId, out var tags))
        tags = new List<TagDb>();
      if (!ResultsRepository.TryGetPhotos(_context, question.UserId, question.QuestionId, 1, photosAmount, maxAge.WhenBorned(), minAge.WhenBorned(), sex, city, out var firstPhotos))
        firstPhotos = new List<PhotoDb>();
      if (!ResultsRepository.TryGetPhotos(_context, question.UserId, question.QuestionId, 2, photosAmount, maxAge.WhenBorned(), minAge.WhenBorned(), sex, city, out var secondPhotos))
        secondPhotos = new List<PhotoDb>();
      if (!CommentsRepository.TryGetComments(_context, question.UserId, question.QuestionId, commentsAmount, out var comments))
        comments = new List<CommentDb>();

      var result = question.MapToGetQuestionsViewModel(tags, firstPhotos, secondPhotos, comments);

      return Ok(result);
    }

    [HttpPost("addQuestion")]
    public IActionResult AddQuestion([FromBody]AddQuestionViewModel question)
    {
      if (question == null)
        return BadRequest($"Input parameter  is null");

      if (!QuestionRepository.TryAddQuestion(_context, question.UserId, question.Condition, question.BackgroundImageLink, question.IsAnonymity ? 1:0, question.IsAudience ? 1 : 0, question.QuestionType, question.FirstOption, question.SecondOption, out var questionId))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var badAddedTags = new List<string>();

      for (var i = 0; i < question.Tags.Count; i++)
      {
        var tag = question.Tags[i];
        if (!TagsRepository.TryAddTag(_context, questionId, tag, i))
          badAddedTags.Add(tag);
      }
      if (badAddedTags.Count != 0)
      {
        var response =
            new
            {
              Message = "Not All Tags inserted",
              BadTegs = badAddedTags
            };
        return BadRequest(response);
      }
      return Ok(questionId);
    }

    [HttpPost("deleteQuestion")]
    public IActionResult DeleteQuestion([FromBody]QuestionIdViewModel questionId)
    {
      if (questionId == null)
        return BadRequest($"Input parameter  is null");

      if (!QuestionRepository.TryDeleteQuestion(_context, questionId.QuestionId, out var isChanged))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      return Ok(isChanged);
    }


    [HttpPost("updateQuestionFeedback")]
    public IActionResult UpdateFeedback([FromBody]UpdateQuestionFeedbackViewModel feedback)
    {
      if (QuestionRepository.TryUpdateQuestionFeedback(_context, feedback.UserId, feedback.QuestionId, feedback.FeedbackType, out var isChanged))
        return Ok(isChanged);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }


    [HttpPost("updateSaved")]
    public IActionResult UpdateSaved([FromBody]UpdateQuestionFavoriteViewModel favorite)
    {
      if (QuestionRepository.TryUpdateSaved(_context, favorite.UserId, favorite.QuestionId, favorite.IsInFavorites, out var isChanged))
        return Ok(isChanged);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("updateFavorites")]
    public IActionResult UpdateFavorites([FromBody]UpdateQuestionFavoriteViewModel favorite)
    {
      if (QuestionRepository.TryUpdateFavorites(_context, favorite.UserId, favorite.QuestionId, favorite.IsInFavorites, out var isChanged))
        return Ok(isChanged);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("updateAnswer")]
    public IActionResult UpdateAnswer([FromBody]UpdateQuestionAnswerViewModel answer)
    {
      if (QuestionRepository.TryUpdateAnswer(_context, answer.UserId, answer.QuestionId, answer.AnswerType, out var isChanged))
        return Ok(isChanged);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("addComplaint")]
    public IActionResult AddComplaint([FromBody]AddComplaintViewModel complaint)
    {
      if (complaint == null)
        return BadRequest($"Input parameter  is null");

      if (ComplaintsRepository.TryAddComplaint(_context, complaint.UserId, complaint.QuestionId, complaint.ComplaintId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("addRecommendedQuestion")]
    public IActionResult AddRecommendedQuestion([FromBody]AddRecommendedQuestionViewModel recommendedQuestion)
    {
      if (recommendedQuestion == null)
        return BadRequest($"Input parameter  is null");

      if (UserQuestionsRepository.TryAddRecommendedQuestion(_context, recommendedQuestion.UserToId, recommendedQuestion.UserFromId, recommendedQuestion.QuestionId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    // только модератору можно
    [HttpPost("getComplaints")]
    public IActionResult GetComplaints()
    {
      if (ComplaintsRepository.TryGetComplaints(_context, out IEnumerable<ComplaintDb> complaints))
        return Ok(complaints);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [Authorize(Roles = "moderator, admin")]
    [HttpPost("getComplaintsAuth")]
    public IActionResult GetComplaintsAuth()
    {
      if (ComplaintsRepository.TryGetComplaints(_context, out IEnumerable<ComplaintDb> complaints))
        return Ok(complaints);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    //[HttpPost("getQuestionResults")]
    //public IActionResult GetQuestionResults([FromBody]GetQuestionResultsViewModel question)
    //{
    //    if (question == null)
    //        return BadRequest($"Input parameter  is null");

    //    if (!ResultsWrapper.TryGetResults(_context, question.UserId, question.QuestionId, question.MinAge, question.MaxAge, question.Sex, out var results))
    //        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

    //    return Ok(results);

    //}

    [HttpPost("getVoters")]
    public IActionResult GetVoters([FromBody]GetVoters voters)
    {
      if (voters == null)
        return BadRequest($"Input parameter  is null");

      if (!ResultsRepository.TryGetVoters(_context, voters.UserId, voters.QuestionId, voters.PageParams.Offset, voters.PageParams.Count, voters.AnswerType, DateTime.UtcNow.AddYears(-voters.MaxAge), DateTime.UtcNow.AddYears(-voters.MinAge), voters.SexType, voters.SearchedLogin, out var answeredList))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      return Ok(answeredList.MapAnsweredListDbToViewModel());

    }
  }
}
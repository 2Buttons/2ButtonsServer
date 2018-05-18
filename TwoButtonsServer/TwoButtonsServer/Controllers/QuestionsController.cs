using System;
using System.Collections.Generic;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.Moderators;
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
    private readonly TwoButtonsUnitOfWork _mainDb;

    public QuestionsController(TwoButtonsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    [HttpPost("getQuestion")]
    public IActionResult GetQuestion([FromBody] QuestionIdViewModel inputQuestion)
    {
      if (inputQuestion == null)
        return BadRequest($"Input parameter  is null");

      if (!_mainDb.Questions.TryGetQuestion(inputQuestion.UserId, inputQuestion.QuestionId, out var question))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");


      var commentsAmount = 10000;
      var photosAmount = 100;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      if (!_mainDb.Tags.TryGetTags(question.QuestionId, out var tags))
        tags = new List<TagDb>();
      if (!_mainDb.Questions.TryGetPhotos(question.UserId, question.QuestionId, 1, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city, out var firstPhotos))
        firstPhotos = new List<PhotoDb>();
      if (!_mainDb.Questions.TryGetPhotos(question.UserId, question.QuestionId, 2, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city, out var secondPhotos))
        secondPhotos = new List<PhotoDb>();
      if (!_mainDb.Comments.TryGetComments(question.UserId, question.QuestionId, commentsAmount, out var comments))
        comments = new List<CommentDb>();

      var result = question.MapToGetQuestionsViewModel(tags, firstPhotos, secondPhotos, comments);

      return Ok(result);
    }

    [HttpPost("addQuestion")]
    public IActionResult AddQuestion([FromBody] AddQuestionViewModel question)
    {
      if (question == null)
        return BadRequest($"Input parameter  is null");

      if (!_mainDb.Questions.TryAddQuestion(question.UserId, question.Condition, question.BackgroundImageLink,
        question.IsAnonymity ? 1 : 0, question.IsAudience ? 1 : 0, question.QuestionType, question.FirstOption,
        question.SecondOption, out var questionId))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var badAddedTags = new List<string>();

      for (var i = 0; i < question.Tags.Count; i++)
      {
        var tag = question.Tags[i];
        if (!_mainDb.Tags.TryAddTag(questionId, tag, i))
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
    public IActionResult DeleteQuestion([FromBody] QuestionIdViewModel questionId)
    {
      if (questionId == null)
        return BadRequest($"Input parameter  is null");

      if (!_mainDb.Questions.TryDeleteQuestion(questionId.QuestionId, out var isChanged))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      return Ok(isChanged);
    }


    [HttpPost("updateQuestionFeedback")]
    public IActionResult UpdateFeedback([FromBody] UpdateQuestionFeedbackViewModel feedback)
    {
      if (_mainDb.Questions.TryUpdateQuestionFeedback(feedback.UserId, feedback.QuestionId, feedback.FeedbackType,
        out var isChanged))
        return Ok(isChanged);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }


    [HttpPost("updateSaved")]
    public IActionResult UpdateSaved([FromBody] UpdateQuestionFavoriteViewModel favorite)
    {
      if (_mainDb.Questions.TryUpdateSaved(favorite.UserId, favorite.QuestionId, favorite.IsInFavorites,
        out var isChanged))
        return Ok(isChanged);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("updateFavorites")]
    public IActionResult UpdateFavorites([FromBody] UpdateQuestionFavoriteViewModel favorite)
    {
      if (_mainDb.Questions.TryUpdateFavorites(favorite.UserId, favorite.QuestionId, favorite.IsInFavorites,
        out var isChanged))
        return Ok(isChanged);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("updateAnswer")]
    public IActionResult UpdateAnswer([FromBody] UpdateQuestionAnswerViewModel answer)
    {
      if (_mainDb.Questions.TryUpdateAnswer(answer.UserId, answer.QuestionId, answer.AnswerType, out var isChanged))
        return Ok(isChanged);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("addComplaint")]
    public IActionResult AddComplaint([FromBody] AddComplaintViewModel complaint)
    {
      if (complaint == null)
        return BadRequest($"Input parameter  is null");

      if (_mainDb.Complaints.TryAddComplaint(complaint.UserId, complaint.QuestionId, complaint.ComplaintId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("addRecommendedQuestion")]
    public IActionResult AddRecommendedQuestion([FromBody] AddRecommendedQuestionViewModel recommendedQuestion)
    {
      if (recommendedQuestion == null)
        return BadRequest($"Input parameter  is null");

      if (_mainDb.UserQuestions.TryAddRecommendedQuestion(recommendedQuestion.UserToId, recommendedQuestion.UserFromId,
        recommendedQuestion.QuestionId))
        return Ok();
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    // только модератору можно
    [HttpPost("getComplaints")]
    public IActionResult GetComplaints()
    {
      if (_mainDb.Complaints.TryGetComplaints(out IEnumerable<ComplaintDb> complaints))
        return Ok(complaints);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [Authorize(Roles = "moderator, admin")]
    [HttpPost("getComplaintsAuth")]
    public IActionResult GetComplaintsAuth()
    {
      if (_mainDb.Complaints.TryGetComplaints(out IEnumerable<ComplaintDb> complaints))
        return Ok(complaints);
      return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("getVoters")]
    public IActionResult GetVoters([FromBody] GetVoters voters)
    {
      if (voters == null)
        return BadRequest($"Input parameter  is null");

      if (!_mainDb.Questions.TryGetVoters(voters.UserId, voters.QuestionId, voters.PageParams.Offset,
        voters.PageParams.Count, voters.AnswerType, DateTime.UtcNow.AddYears(-voters.MaxAge),
        DateTime.UtcNow.AddYears(-voters.MinAge), voters.SexType, voters.SearchedLogin, out var answeredList))
        return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      return Ok(answeredList.MapAnsweredListDbToViewModel());
    }
  }
}
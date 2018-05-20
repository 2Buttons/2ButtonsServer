using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuestionsData;
using QuestionsData.Entities;
using QuestionsServer.ViewModels.InputParameters;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;
using QuestionsServer.ViewModels.OutputParameters;

namespace QuestionsServer.Controllers
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
    public async Task<IActionResult> GetQuestion([FromBody] QuestionIdViewModel inputQuestion)
    {
      if (inputQuestion == null)
        return BadRequest($"Input parameter  is null");

      var question = await _mainDb.Questions.GetQuestion(inputQuestion.UserId, inputQuestion.QuestionId);
      // return BadRequest("Something goes wrong. We will fix it!... maybe)))");


      GetTagsAndPhotos(inputQuestion.UserId, inputQuestion.QuestionId, out var tags, out var firstPhotos, out var secondPhotos, out var comments);

      var result = question.MapToGetQuestionsViewModel(tags, firstPhotos, secondPhotos, comments);

      return Ok(result);
    }

    private void GetTagsAndPhotos(int userId, int questionId, out IEnumerable<TagDb> tags,
      out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos, out IEnumerable<CommentDb> comments)
    {
      var photosAmount = 100;
      var commentsAmount = 500;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      var tagsTask = _mainDb.Tags.GetTags(questionId);
      var firstPhotosTask = _mainDb.Questions.GetPhotos(userId, questionId, 1, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city);
      var secondPhotosTask = _mainDb.Questions.GetPhotos(userId, questionId, 2, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city);
      var commentsTask = _mainDb.Comments.GetComments(userId, questionId, commentsAmount);

      Task.WhenAll(tagsTask, firstPhotosTask, secondPhotosTask, commentsTask);
      tags = tagsTask.Result;
      firstPhotos = firstPhotosTask.Result;
      secondPhotos = secondPhotosTask.Result;
      comments = commentsTask.Result;
    }

    [HttpPost("addQuestion")]
    public async Task<IActionResult> AddQuestion([FromBody] AddQuestionViewModel question)
    {
      if (question == null)
        return BadRequest($"Input parameter  is null");

      var questionId = await _mainDb.Questions.AddQuestion(question.UserId, question.Condition,
        question.BackgroundImageLink,
        question.IsAnonymity ? 1 : 0, question.IsAudience ? 1 : 0, question.QuestionType, question.FirstOption,
        question.SecondOption);
      //return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      var badAddedTags = new List<string>();

      for (var i = 0; i < question.Tags.Count; i++)
      {
        var tag = question.Tags[i];
        if (await _mainDb.Tags.AddTag(questionId, tag, i))
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
    public async Task<IActionResult> DeleteQuestion([FromBody] QuestionIdViewModel questionId)
    {
      if (questionId == null)
        return BadRequest($"Input parameter  is null");

      var isChanged = await _mainDb.Questions.DeleteQuestion(questionId.QuestionId);
      //return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      return Ok(isChanged);
    }


    [HttpPost("updateQuestionFeedback")]
    public async Task<IActionResult> UpdateFeedback([FromBody] UpdateQuestionFeedbackViewModel feedback)
    {
      var isChanged =
        await _mainDb.Questions.UpdateQuestionFeedback(feedback.UserId, feedback.QuestionId, feedback.FeedbackType);
      return Ok(isChanged);
      // return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }


    [HttpPost("updateSaved")]
    public async Task<IActionResult> UpdateSaved([FromBody] UpdateQuestionFavoriteViewModel favorite)
    {
      var isChanged = await _mainDb.Questions.UpdateSaved(favorite.UserId, favorite.QuestionId, favorite.IsInFavorites);
      return Ok(isChanged);
      //return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("updateFavorites")]
    public async Task<IActionResult> UpdateFavorites([FromBody] UpdateQuestionFavoriteViewModel favorite)
    {
      var isChanged =
        await _mainDb.Questions.UpdateFavorites(favorite.UserId, favorite.QuestionId, favorite.IsInFavorites);
      return Ok(isChanged);
      //return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("updateAnswer")]
    public async Task<IActionResult> UpdateAnswer([FromBody] UpdateQuestionAnswerViewModel answer)
    {
      var isChanged = await _mainDb.Questions.UpdateAnswer(answer.UserId, answer.QuestionId, answer.AnswerType);
      return Ok(isChanged);
      // return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("addComplaint")]
    public async Task<IActionResult> AddComplaint([FromBody] AddComplaintViewModel complaint)
    {
      if (complaint == null)
        return BadRequest($"Input parameter  is null");

      await _mainDb.Complaints.AddComplaint(complaint.UserId, complaint.QuestionId, complaint.ComplaintId);
      return Ok();
      //return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("addRecommendedQuestion")]
    public async Task<IActionResult> AddRecommendedQuestion(
      [FromBody] AddRecommendedQuestionViewModel recommendedQuestion)
    {
      if (recommendedQuestion == null)
        return BadRequest($"Input parameter  is null");

      var questionId = await _mainDb.UserQuestions.AddRecommendedQuestion(recommendedQuestion.UserToId,
        recommendedQuestion.UserFromId,
        recommendedQuestion.QuestionId);
      return Ok();
      // return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    // только модератору можно
    [HttpPost("getComplaints")]
    public async Task<IActionResult> GetComplaints()
    {
      var complaints = await _mainDb.Complaints.GetComplaints();
      return Ok(complaints);
      // return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [Authorize(Roles = "moderator, admin")]
    [HttpPost("getComplaintsAuth")]
    public async Task<IActionResult> GetComplaintsAuth()
    {
      var complaints = await _mainDb.Complaints.GetComplaints();
      return Ok(complaints);
      // return BadRequest("Something goes wrong. We will fix it!... maybe)))");
    }

    [HttpPost("getVoters")]
    public async Task<IActionResult> GetVoters([FromBody] GetVoters voters)
    {
      if (voters == null)
        return BadRequest($"Input parameter  is null");

      var answeredList = await _mainDb.Questions.GetVoters(voters.UserId, voters.QuestionId, voters.PageParams.Offset,
        voters.PageParams.Count, voters.AnswerType, DateTime.UtcNow.AddYears(-voters.MaxAge),
        DateTime.UtcNow.AddYears(-voters.MinAge), voters.SexType, voters.SearchedLogin);
      // return BadRequest("Something goes wrong. We will fix it!... maybe)))");

      return Ok(answeredList.MapAnsweredListDbToViewModel());
    }
  }
}
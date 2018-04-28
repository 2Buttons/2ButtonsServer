using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.Moderators;
using TwoButtonsDatabase.WrapperFunctions;
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

        [HttpPost("addQuestion")]
        public IActionResult AddQuestion([FromBody]AddQuestionViewModel question)
        {
            if (question == null)
                return BadRequest($"Input parameter  is null");

            if (!QuestionWrapper.TryAddQuestion(_context, question.UserId, question.Condition, question.BackgroundImageLink, question.Anonymity, question.Audience, question.QuestionType, question.FirstOption, question.SecondOption, out var questionId))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var badAddedTags = new List<TagViewModel>();

            foreach (var tag in question.Tags)
            {
                if (!TagsWrapper.TryAddTag(_context, questionId, tag.TagText, tag.Position))
                    badAddedTags.Add(tag);
            }
            if (badAddedTags.Count != 0)
            {
                var response =
                    new
                    {
                        Message= "Not All Tags inserted",
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

            if (!QuestionWrapper.TryDeleteQuestion(_context, questionId.QuestionId))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");
          
            return Ok();
        }

        [HttpPost("addAnswer")]
        public IActionResult AddAnswer([FromBody]AddAnswerViewModel answer)
        {
            if (answer == null)
                return BadRequest($"Input parameter  is null");

            if (QuestionWrapper.TryAddAnswer( _context, answer.UserId, answer.QuestionId, answer.Answer, answer.YourFeedback))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("addComplaint")]
        public IActionResult AddComplaint([FromBody]AddComplaintViewModel complaint)
        {
            if (complaint == null)
                return BadRequest($"Input parameter  is null");

            if (ModeratorWrapper.TryAddComplaint(_context, complaint.UserId, complaint.QuestionId, complaint.ComplaintId))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }


        [HttpPost("addRecommendedQuestion")]
        public IActionResult AddRecommendedQuestion([FromBody]AddRecommendedQuestionViewModel recommendedQuestion)
        {
            if (recommendedQuestion == null)
                return BadRequest($"Input parameter  is null");

            if (UserQuestionsWrapper.TryAddRecommendedQuestion(_context, recommendedQuestion.UserToId, recommendedQuestion.UserFromId, recommendedQuestion.QuestionId))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        // только модератору можно
        [HttpPost("getComplaints")]
        public IActionResult GetComplaints()
        {

            if (ModeratorWrapper.TryGetComplaints(_context, out IEnumerable<ComplaintDb> complaints))
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

            if (!ResultsWrapper.TryGetAnsweredList(_context, voters.UserId, voters.QuestionId,voters.PageParams.Page, voters.PageParams.Amount, voters.Option, voters.MinAge, voters.MaxAge, voters.Sex, voters.SearchedLogin, out var answeredList))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            return Ok(answeredList.MapAnsweredListDbToViewModel());
            
        }
    }
}
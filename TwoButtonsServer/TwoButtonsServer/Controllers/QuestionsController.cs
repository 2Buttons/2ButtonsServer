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
using TwoButtonsServer.ViewModels.InputParameters;
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;

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
            if (!QuestionWrapper.TryAddQuestion(_context, question.UserId, question.Condition, question.Anonymity, question.Audience, question.QuestionType, question.StandartPictureId, question.FirstOption, question.SecondOption, question.BackgroundImageLink, out var questionId))
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
            return Ok();   
        }

        [HttpPost("addAnswer")]
        public IActionResult AddAnswer([FromBody]AddAnswerViewModel answer)
        {
            if (QuestionWrapper.TryAddAnswer( _context, answer.UserId, answer.QuestionId, answer.Answer, answer.YourFeedback))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("addComplaint")]
        public IActionResult AddComplaint([FromBody]AddComplaintViewModel complaint)
        {
            if (ModeratorWrapper.TryAddComplaint(_context, complaint.UserId, complaint.QuestionId, complaint.ComplaintId))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("addTag")]
        public IActionResult AddTag([FromBody]AddTagViewModel tag)
        {
            if (TagsWrapper.TryAddTag(_context, tag.QuestionId, tag.TagText, tag.Position))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("addRecommendedQuestion")]
        public IActionResult AddRecommendedQuestion([FromBody]AddRecommendedQuestionViewModel recommendedQuestion)
        {
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

        [HttpPost("getQuestionResults")]
        public IActionResult GetQuestionResults([FromBody]GetQuestionResultsViewModel question)
        {
            if (ResultsWrapper.TryGetResults(_context, question.UserId, question.QuestionId, question.MinAge, question.MaxAge, question.Sex, out var results))
                return Ok(results);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("getVoters")]
        public IActionResult GetVoters([FromBody]GetVoters voters)
        {
            if (ResultsWrapper.TryGetAnsweredList(_context, voters.UserId, voters.QuestionId, voters.VotersAmount, voters.Option, voters.MinAge, voters.MaxAge, voters.Sex, voters.SearchedLogin, out var answeredList))
                return Ok(answeredList);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("saveFeedback")]
        public IActionResult SaveFeedback([FromBody]SaveFeedbackViewModel feedback)
        {
            if (QuestionWrapper.TrySaveFeedback(_context, feedback.UserId, feedback.QuestionId, feedback.Feeback))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

       
        [HttpPost("saveFavorites")]
        public IActionResult SaveFavorites([FromBody]SaveFavoritesViewModel favorites)
        {
            if (QuestionWrapper.TrySaveFeedback(_context, favorites.UserId, favorites.QuestionId, favorites.InFavorites))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

   
        [HttpPost("saveAnswer")]
        public IActionResult SaveAnswer([FromBody]SaveAnswerViewModel answer)
        {
            if (QuestionWrapper.TrySaveFeedback(_context, answer.UserId, answer.QuestionId, answer.Answer))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }
    }
}
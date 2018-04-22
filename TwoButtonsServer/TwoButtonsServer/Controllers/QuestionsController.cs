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
        public IActionResult AddQuestion(int userId, string condition,  int anonymity, int audience, int questionType, int standartPictureId, string firstOption, string secondOption, string backgroundImageLink = null)
        {
            if (QuestionWrapper.TryAddQuestion(_context, userId, condition, anonymity, audience, questionType, standartPictureId, firstOption, secondOption, backgroundImageLink))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("addAnswer")]
        public IActionResult AddAnswer(int userId, int questionId, string answer, int yourFeedback)
        {
            if (QuestionWrapper.TryAddAnswer( _context,  userId,  questionId,  answer,  yourFeedback))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("addComplaint")]
        public IActionResult AddComplaint(int userId, int questionId, int complaintId)
        {
            if (ModeratorWrapper.TryAddComplaint(_context,  userId, questionId, complaintId))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("addTag")]
        public IActionResult AddTag(int questionId, string tagText, int position)
        {
            if (TagsWrapper.TryAddTag(_context, questionId,  tagText,  position, out int tagId))
                return Ok(tagId);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("addRecommendedQuestion")]
        public IActionResult AddRecommendedQuestion(int userToId, int userFromId, int questionId)
        {
            if (UserQuestionsWrapper.TryAddRecommendedQuestion(_context,  userToId,  userFromId,  questionId))
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
        public IActionResult GetQuestionResults(int id, int questionId, int minAge = 0, int maxAge = 100, int sex = 0)
        {
            if (ResultsWrapper.TryGetResults(_context, id, questionId, minAge, maxAge, sex, out var results))
                return Ok(results);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

        [HttpPost("saveFeedback")]
        public IActionResult SaveFeedback(int id, int questionId, int feedback=0)
        {
            if (QuestionWrapper.TrySaveFeedback(_context,id, questionId, feedback))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

       
        [HttpPost("saveFavorites")]
        public IActionResult SaveFavorites(int id, int questionId, int inFavorites=0)
        {
            if (QuestionWrapper.TrySaveFeedback(_context, id, questionId, inFavorites))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }

   
        [HttpPost("saveAnswer")]
        public IActionResult SaveAnswer(int id, int questionId, int answer)
        {
            if (QuestionWrapper.TrySaveFeedback(_context, id, questionId, answer))
                return Ok();
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }
    }
}
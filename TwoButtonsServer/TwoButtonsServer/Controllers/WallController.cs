using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.WrapperFunctions;
using TwoButtonsServer.ViewModels;
using TwoButtonsServer.ViewModels.UserQuestions;

namespace TwoButtonsServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class WallController : Controller //To get user's posts
    {
        private readonly TwoButtonsContext _context;
        public WallController(TwoButtonsContext context)
        {
            _context = context;
        }

        [HttpPost("getNotifications")]
        public IActionResult GetNotifications(int userId)
        {
            if (!NotificationsWrapper.TryGetNotifications(_context, userId, out var notifications))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");
           
            return Ok(notifications.MapNotificationDbToViewModel());
        }

        [HttpPost("getPosts")]
        public IActionResult GetPosts(int userId, int pageUserId, int amount = 100)
        {
            if (UserWrapper.TryGetPosts(_context, userId, pageUserId, amount, out var posts))
                return Ok(posts);
            return BadRequest("Something goes wrong. We will fix it!... maybe)))");
        }


        [HttpPost("getUserAskedQuestions")]
        public IActionResult GetUserAskedQuestions(int userId, int pageUserId, int questionsAmount = 100, int photosAmount = 100, int minAge = 0, int maxAge = 100, int sex = 0)
        {
            if (!UserQuestionsWrapper.TryGetUserAskedQuestions(_context, userId, pageUserId, questionsAmount, out var userAskedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserAskedQuestionsViewModel>();

            foreach (var question in userAskedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToUserAskedQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);

        }


        [HttpPost("getUserAnsweredQuestions")]
        public IActionResult GetUserAnsweredQuestions(int userId, int pageUserId, int questionsAmount = 100, int photosAmount = 100, int minAge = 0, int maxAge = 100, int sex = 0)
        {
            if (!UserQuestionsWrapper.TryGetUserAnsweredQuestions(_context, userId, pageUserId, questionsAmount, out var userAnsweredQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserAnsweredQuestionsViewModel>();

            foreach (var question in userAnsweredQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToUserAnsweredQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }

        [HttpPost("getUserFavoriteQuestions")]
        public IActionResult GetUserFavoriteQuestions(int userId, int pageUserId, int questionsAmount = 100, int photosAmount = 100, int minAge = 0, int maxAge = 100, int sex = 0)
        {
            if (!UserQuestionsWrapper.TryGetUserFavoriteQuestions(_context, userId, pageUserId, questionsAmount, out var userFavouriteQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserFavouriteQuestionsViewModel>();

            foreach (var question in userFavouriteQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToUserFavouriteQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }


        [HttpPost("getUserCommentedQuestions")]
        public IActionResult GetUserCommentedQuestions(int userId, int pageUserId, int questionsAmount = 100, int photosAmount = 100, int minAge = 0, int maxAge = 100, int sex = 0)
        {
            if (!UserQuestionsWrapper.TryGetUserCommentedQuestions(_context, userId, pageUserId, questionsAmount, out var userCommentedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserCommentedQuestionsViewModel>();

            foreach (var question in userCommentedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToUserCommentedQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }


        [HttpPost("getTopQuestions")]
        public IActionResult GetTopQuestions(int userId, DateTime topAfterDate, bool isOnlyNew = true, int questionsAmount = 100, int photosAmount = 100, int minAge = 0, int maxAge = 100, int sex = 0)
        {

            if (!UserQuestionsWrapper.TryGeTopQuestions(_context, userId, isOnlyNew, questionsAmount, topAfterDate, out var userTopQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<TopQuestionsViewModel>();

            foreach (var question in userTopQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToTopQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }


        [HttpPost("getAskedQuestions")]
        public IActionResult TryGetAskedQuestions(int userId, int pageUserId, int questionsAmount = 100, int photosAmount = 100, int minAge = 0, int maxAge = 100, int sex = 0)
        {
            if (!UserQuestionsWrapper.TryGetAskedQuestions(_context, userId, pageUserId, questionsAmount, out var userAskedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<AskedQuestionsViewModel>();

            foreach (var question in userAskedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToAskedQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);

        }


        [HttpPost("getLikedQuestions")]
        public IActionResult TryGetLikedQuestions(int userId, int pageUserId, int questionsAmount = 100, int photosAmount = 100, int minAge = 0, int maxAge = 100, int sex = 0)
        {
            if (!UserQuestionsWrapper.TryGetLikedQuestions(_context, userId, pageUserId, questionsAmount, out var userAnsweredQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<LikedQuestionsViewModel>();

            foreach (var question in userAnsweredQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToLikedQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }

        [HttpPost("getSavedQuestions")]
        public IActionResult TryGetSavedQuestions(int userId, int questionsAmount = 100, int photosAmount = 100, int minAge = 0, int maxAge = 100, int sex = 0)
        {
            if (!UserQuestionsWrapper.TryGetSavedQuestions(_context, userId, questionsAmount, out var userFavouriteQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<SavedQuestionsViewModel>();

            foreach (var question in userFavouriteQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToSavedQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }
    }
}
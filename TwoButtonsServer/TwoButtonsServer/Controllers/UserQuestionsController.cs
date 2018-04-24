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
using TwoButtonsServer.ViewModels.InputParameters;
using TwoButtonsServer.ViewModels.OutputParameters;
using TwoButtonsServer.ViewModels.OutputParameters.UserQuestions;

namespace TwoButtonsServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class UserQuestionsController : Controller //To get user's posts
    {
        private readonly TwoButtonsContext _context;
        public UserQuestionsController(TwoButtonsContext context)
        {
            _context = context;
        }

       
        [HttpPost("getUserAskedQuestions")]
        public IActionResult GetUserAskedQuestions([FromBody]UserQuestionsViewModel userQuestions)
        {
            if (!UserQuestionsWrapper.TryGetUserAskedQuestions(_context, userQuestions.UserId, userQuestions.UserPageId, userQuestions.QuestionsAmount, out var userAskedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserAskedQuestionsViewModel>();

            int photosAmount = userQuestions.PhotoParams.PhotosAmount;
            int minAge = userQuestions.PhotoParams.MinAge;
            int maxAge = userQuestions.PhotoParams.MaxAge;
            int sex = userQuestions.PhotoParams.Sex;

            foreach (var question in userAskedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToUserAskedQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);

        }


        [HttpPost("getUserAnsweredQuestions")]
        public IActionResult GetUserAnsweredQuestions([FromBody]UserQuestionsViewModel userQuestions)
        {
            if (!UserQuestionsWrapper.TryGetUserAnsweredQuestions(_context, userQuestions.UserId, userQuestions.UserPageId, userQuestions.QuestionsAmount, out var userAnsweredQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserAnsweredQuestionsViewModel>();


            int photosAmount = userQuestions.PhotoParams.PhotosAmount;
            int minAge = userQuestions.PhotoParams.MinAge;
            int maxAge = userQuestions.PhotoParams.MaxAge;
            int sex = userQuestions.PhotoParams.Sex;

            foreach (var question in userAnsweredQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToUserAnsweredQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }

        [HttpPost("getUserFavoriteQuestions")]
        public IActionResult GetUserFavoriteQuestions([FromBody]UserQuestionsViewModel userQuestions)
        {
            if (!UserQuestionsWrapper.TryGetUserFavoriteQuestions(_context, userQuestions.UserId, userQuestions.UserPageId, userQuestions.QuestionsAmount, out var userFavouriteQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserFavouriteQuestionsViewModel>();


            int photosAmount = userQuestions.PhotoParams.PhotosAmount;
            int minAge = userQuestions.PhotoParams.MinAge;
            int maxAge = userQuestions.PhotoParams.MaxAge;
            int sex = userQuestions.PhotoParams.Sex;

            foreach (var question in userFavouriteQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToUserFavouriteQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }


        [HttpPost("getUserCommentedQuestions")]
        public IActionResult GetUserCommentedQuestions([FromBody]UserQuestionsViewModel userQuestions)
        {
            if (!UserQuestionsWrapper.TryGetUserCommentedQuestions(_context, userQuestions.UserId, userQuestions.UserPageId, userQuestions.QuestionsAmount, out var userCommentedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<UserCommentedQuestionsViewModel>();


            int photosAmount = userQuestions.PhotoParams.PhotosAmount;
            int minAge = userQuestions.PhotoParams.MinAge;
            int maxAge = userQuestions.PhotoParams.MaxAge;
            int sex = userQuestions.PhotoParams.Sex;

            foreach (var question in userCommentedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToUserCommentedQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }


        [HttpPost("getTopQuestions")]
        public IActionResult GetTopQuestions([FromBody]TopUserQuestions userQuestions)
        {

            if (!UserQuestionsWrapper.TryGeTopQuestions(_context, userQuestions.UserId, userQuestions.IsOnlyNew, userQuestions.QuestionsAmount, userQuestions.TopAfterDate, out var userTopQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<TopQuestionsViewModel>();


            int photosAmount = userQuestions.PhotoParams.PhotosAmount;
            int minAge = userQuestions.PhotoParams.MinAge;
            int maxAge = userQuestions.PhotoParams.MaxAge;
            int sex = userQuestions.PhotoParams.Sex;

            foreach (var question in userTopQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToTopQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }


        [HttpPost("getAskedQuestions")]
        public IActionResult TryGetAskedQuestions([FromBody]UserQuestionsViewModel userQuestions)
        {
            if (!UserQuestionsWrapper.TryGetAskedQuestions(_context, userQuestions.UserId, userQuestions.UserPageId, userQuestions.QuestionsAmount, out var userAskedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<AskedQuestionsViewModel>();


            int photosAmount = userQuestions.PhotoParams.PhotosAmount;
            int minAge = userQuestions.PhotoParams.MinAge;
            int maxAge = userQuestions.PhotoParams.MaxAge;
            int sex = userQuestions.PhotoParams.Sex;

            foreach (var question in userAskedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToAskedQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);

        }


        [HttpPost("getLikedQuestions")]
        public IActionResult TryGetLikedQuestions([FromBody]UserQuestionsViewModel userQuestions)
        {
            if (!UserQuestionsWrapper.TryGetLikedQuestions(_context, userQuestions.UserId, userQuestions.UserPageId, userQuestions.QuestionsAmount, out var userAnsweredQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<LikedQuestionsViewModel>();


            int photosAmount = userQuestions.PhotoParams.PhotosAmount;
            int minAge = userQuestions.PhotoParams.MinAge;
            int maxAge = userQuestions.PhotoParams.MaxAge;
            int sex = userQuestions.PhotoParams.Sex;

            foreach (var question in userAnsweredQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToLikedQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }

        [HttpPost("getSavedQuestions")]
        public IActionResult TryGetSavedQuestions([FromBody]UserQuestionsViewModel userQuestions)
        {
            if (!UserQuestionsWrapper.TryGetSavedQuestions(_context, userQuestions.UserId, userQuestions.QuestionsAmount, out var userFavouriteQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<SavedQuestionsViewModel>();


            int photosAmount = userQuestions.PhotoParams.PhotosAmount;
            int minAge = userQuestions.PhotoParams.MinAge;
            int maxAge = userQuestions.PhotoParams.MaxAge;
            int sex = userQuestions.PhotoParams.Sex;

            foreach (var question in userFavouriteQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
                    firstPhotos = new List<PhotoDb>();
                if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
                    secondPhotos = new List<PhotoDb>();
                result.Add(question.MapToSavedQuestionsViewModel(tags, firstPhotos, secondPhotos));
            }
            return Ok(result);
        }

        //private List<TResult> GetLists<T, TResult>(IEnumerable<QuestionBaseDb> questionList, PhotoParamsViewModel photoParams) 
        //{
        //    var result = new List<SavedQuestionsViewModel>();


        //    int photosAmount = photoParams.PhotosAmount;
        //    int minAge = photoParams.MinAge;
        //    int maxAge = photoParams.MaxAge;
        //    int sex = photoParams.Sex;

        //    foreach (var question in questionList)
        //    {
        //        if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
        //            tags = new List<TagDb>();
        //        if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 1, photosAmount, minAge, maxAge, sex, out var firstPhotos))
        //            firstPhotos = new List<PhotoDb>();
        //        if (!ResultsWrapper.TryGetPhotos(_context, userQuestions.UserId, question.QuestionId, 2, photosAmount, minAge, maxAge, sex, out var secondPhotos))
        //            secondPhotos = new List<PhotoDb>();
        //        result.Add(question.MapToSavedQuestionsViewModel(tags, firstPhotos, secondPhotos));
        //    }
        //}
    }
}
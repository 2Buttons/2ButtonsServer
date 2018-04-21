using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.WrapperFunctions;
using TwoButtonsServer.ViewModels;
using TwoButtonsServer.ViewModels.News;
using TwoButtonsServer.ViewModels.UserQuestions;


namespace TwoButtonsServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class NewsController : Controller //Don't receive deleted
    {
        private readonly TwoButtonsContext _context;
        public NewsController(TwoButtonsContext context)
        {
            _context = context;
        }
        [HttpPost("getNewsAskedQuestions")]
        public IActionResult GetNewsAskedQuestions(int userId, int amount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsAskedQuestions(_context, userId, amount, out var userAskedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<NewsAskedQuestionsViewModel>();

            foreach (var question in userAskedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                result.Add(question.MapToNewsAskedQuestionsViewModel(tags));
            }
            return Ok(result);

        }


        [HttpPost("getNewsAnsweredQuestions")]
        public IActionResult GetNewsAnsweredQuestions(int userId, int amount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsAnsweredQuestions(_context,  userId, amount,  out var userAnsweredQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<NewsAnsweredQuestionsViewModel>();

            foreach (var question in userAnsweredQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                result.Add(question.MapToNewsAnsweredQuestionsViewModel(tags));
            }
            return Ok(result);
        }

        [HttpPost("getNewsFavoriteQuestions")]
        public IActionResult GetNewsFavoriteQuestions(int userId, int amount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsFavoriteQuestions(_context,userId, amount,  out var userFavouriteQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<NewsFavouriteQuestionsViewModel>();

            foreach (var question in userFavouriteQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                result.Add(question.MapToNewsFavouriteQuestionsViewModel(tags));
            }
            return Ok(result);
        }


        [HttpPost("getNewsCommentedQuestions")]
        public IActionResult GetNewsCommentedQuestions( int userId, int amount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsCommentedQuestions(_context,  userId, amount, out var userCommentedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<NewsCommentedQuestionsViewModel>();

            foreach (var question in userCommentedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                result.Add(question.MapToNewsCommentedQuestionsViewModel(tags));
            }
            return Ok(result);
        }


        [HttpPost("getNewsRecommendedQuestions")]
        public IActionResult TryGetNewsRecommendedQuestions(int userId, int amount = 100)
        {

            if (!NewsQuestionsWrapper.TryGetNewsRecommendedQuestions(_context, userId, amount, out var newsRecommendedQuestions))
                return BadRequest("Something goes wrong. We will fix it!... maybe)))");

            var result = new List<NewsRecommendedQuestionViewModel>();

            foreach (var question in newsRecommendedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                result.Add(question.MapNewsRecommendedQuestionsViewModel(tags));
            }
            return Ok(result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [HttpPost("getNews")]
        public async  Task<IActionResult> GetNews(int userId, int amount = 100)
        {
            var askedList = Task.Run(()=>GetNewsAskedQuestions(userId, amount)).GetAwaiter().GetResult();
            var answeredList = Task.Run(() => GetNewsAnsweredQuestions(userId, amount)).GetAwaiter().GetResult();
            var favouriteList = Task.Run(() => GetNewsFavoriteQuestions(userId, amount)).GetAwaiter().GetResult();
            var commentedList = Task.Run(() => GetNewsCommentedQuestions(userId, amount)).GetAwaiter().GetResult();
            var recommentedList = Task.Run(() => TryGetNewsRecommendedQuestions(userId, amount)).GetAwaiter().GetResult();

           // await Task.WhenAll(new Task[] { askedList, answeredList, favouriteList, commentedList, recommentedList});

            NewsViewModel result = new NewsViewModel
            {
                NewsAskedQuestions = askedList,
                NewsAnsweredQuestions = answeredList,
                NewsFavouriteQuestions = favouriteList,
                NewsCommentedQuestions = commentedList,
                NewsRecommendedQuestions = recommentedList
            };


            return Ok(result);
        }

        private List<NewsAskedQuestionViewModel> GetNewsAskedQuestions(int userId, int amount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsAskedQuestions(_context, userId, amount, out var userAskedQuestions))
                return new List<NewsAskedQuestionViewModel>();

            var result = new List<NewsAskedQuestionViewModel>();

            int index = 0;
            foreach (var question in userAskedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                var resultQuestion = question.MapToNewsAskedQuestionsViewModel(tags);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);

            }
            return result;

        }


        private List<NewsAnsweredQuestionViewModel> GetNewsAnsweredQuestions(int userId, int amount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsAnsweredQuestions(_context, userId, amount,
                out var userAnsweredQuestions))
                return new List<NewsAnsweredQuestionViewModel>();

            var result = new List<NewsAnsweredQuestionViewModel>();
            int index = 0;
            foreach (var question in userAnsweredQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                var resultQuestion = question.MapToNewsAnsweredQuestionsViewModel(tags);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            }
            return result;
        }


        private List<NewsFavouriteQuestionViewModel> GetNewsFavoriteQuestions(int userId, int amount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsFavoriteQuestions(_context, userId, amount,
                out var userFavouriteQuestions))
                return new List<NewsFavouriteQuestionViewModel>();

            var result = new List<NewsFavouriteQuestionViewModel>();
            int index = 0;
            foreach (var question in userFavouriteQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                var resultQuestion = question.MapToNewsFavouriteQuestionsViewModel(tags);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            }
            return result;
        }


        private List<NewsCommentedQuestionViewModel> GetNewsCommentedQuestions( int userId, int amount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsCommentedQuestions(_context, userId, amount,
                out var userCommentedQuestions))
                return new List<NewsCommentedQuestionViewModel>();

            var result = new List<NewsCommentedQuestionViewModel>();
            int index = 0;
            foreach (var question in userCommentedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                var resultQuestion = question.MapToNewsCommentedQuestionsViewModel(tags);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            }
            return result;
        }


        private List<NewsRecommendedQuestionViewModel> TryGetNewsRecommendedQuestions(int userId, int amount = 100)
        {

            if (!NewsQuestionsWrapper.TryGetNewsRecommendedQuestions(_context, userId, amount,
                out var newsRecommendedQuestions))
                return new List<NewsRecommendedQuestionViewModel>();

            var result = new List<NewsRecommendedQuestionViewModel>();
            int index = 0;
            foreach (var question in newsRecommendedQuestions)
            {
                if (!TagsWrapper.TryGetTags(_context, question.QuestionId, out var tags))
                    tags = new List<TagDb>();
                var resultQuestion = question.MapNewsRecommendedQuestionsViewModel(tags);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            }
            return result;
        }
    }
}
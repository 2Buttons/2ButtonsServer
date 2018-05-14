using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.WrapperFunctions;
using TwoButtonsServer.ViewModels;
using TwoButtonsServer.ViewModels.InputParameters;
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;
using TwoButtonsServer.ViewModels.OutputParameters;
using TwoButtonsServer.ViewModels.OutputParameters.NewsQuestions;


namespace TwoButtonsServer.Controllers
{
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class NewsQuestionsController : Controller //Don't receive deleted
    {
        private readonly TwoButtonsContext _context;
        public NewsQuestionsController(TwoButtonsContext context)
        {
            _context = context;
        }

        [HttpPost("getNews")]
        public async  Task<IActionResult> GetNews([FromBody]GetNewsViewModel  newsViewModel)
        {
            if (newsViewModel == null || newsViewModel.PageParams == null)
                return BadRequest($"Input parameter  is null");

            int userId= newsViewModel.UserId;
            int questionsPage = newsViewModel.PageParams.Offset;
            int questionsAmount = newsViewModel.PageParams.Count;

            var askedList = Task.Run(()=>GetNewsAskedQuestions(userId, questionsPage, questionsAmount)).GetAwaiter().GetResult();
            var answeredList = Task.Run(() => GetNewsAnsweredQuestions(userId, questionsPage, questionsAmount)).GetAwaiter().GetResult();
            var favoriteList = Task.Run(() => GetNewsFavoriteQuestions(userId, questionsPage, questionsAmount)).GetAwaiter().GetResult();
            var commentedList = Task.Run(() => GetNewsCommentedQuestions(userId, questionsPage, questionsAmount)).GetAwaiter().GetResult();
            var recommentedList = Task.Run(() => TryGetNewsRecommendedQuestions(userId, questionsPage, questionsAmount)).GetAwaiter().GetResult();

         

            // await Task.WhenAll(new Task[] { askedList, answeredList, favoriteList, commentedList, recommentedList});

            NewsViewModel result = new NewsViewModel
            {
                NewsAskedQuestions = askedList,
                NewsAnsweredQuestions = answeredList,
                NewsFavoriteQuestions = favoriteList,
                NewsCommentedQuestions = commentedList,
                NewsRecommendedQuestions = recommentedList
            };


            return Ok(result);
        }

        private List<NewsAskedQuestionViewModel> GetNewsAskedQuestions(int userId, int questionsPage=1, int questionsAmount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsAskedQuestions(_context, userId, questionsPage, questionsAmount, out var userAskedQuestions))
                return new List<NewsAskedQuestionViewModel>();

            var result = new List<NewsAskedQuestionViewModel>();

            int index = 0;

            Parallel.ForEach(userAskedQuestions, (x) =>
            {
                GetTagsAndPhotos(userId, x.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
                var resultQuestion = x.MapToNewsAskedQuestionsViewModel(tags, firstPhotos, secondPhotos);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            });

            //foreach (var question in userAskedQuestions)
            //{
                

            //}
            return result;

        }


        private List<NewsAnsweredQuestionViewModel> GetNewsAnsweredQuestions(int userId, int questionsPage = 1, int questionsAmount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsAnsweredQuestions(_context, userId, questionsPage, questionsAmount,
                out var userAnsweredQuestions))
                return new List<NewsAnsweredQuestionViewModel>();

            var result = new List<NewsAnsweredQuestionViewModel>();
            int index = 0;
            Parallel.ForEach(userAnsweredQuestions, (question) =>
            {
                GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
                var resultQuestion = question.MapToNewsAnsweredQuestionsViewModel(tags, firstPhotos, secondPhotos);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            });
            return result;
        }


        private List<NewsFavoriteQuestionViewModel> GetNewsFavoriteQuestions(int userId, int questionsPage = 1, int questionsAmount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsFavoriteQuestions(_context, userId, questionsPage, questionsAmount,
                out var userFavoriteQuestions))
                return new List<NewsFavoriteQuestionViewModel>();

            var result = new List<NewsFavoriteQuestionViewModel>();
            int index = 0;
            Parallel.ForEach(userFavoriteQuestions, (question) =>
            {
                GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
                var resultQuestion = question.MapToNewsFavoriteQuestionsViewModel(tags, firstPhotos, secondPhotos);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            });
            return result;
        }


        private List<NewsCommentedQuestionViewModel> GetNewsCommentedQuestions( int userId, int questionsPage = 1, int questionsAmount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsCommentedQuestions(_context, userId, questionsPage, questionsAmount,
                out var userCommentedQuestions))
                return new List<NewsCommentedQuestionViewModel>();

            var result = new List<NewsCommentedQuestionViewModel>();
            int index = 0;
            Parallel.ForEach(userCommentedQuestions, (question) =>
            {
                GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
                var resultQuestion = question.MapToNewsCommentedQuestionsViewModel(tags, firstPhotos, secondPhotos);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            });
            return result;
        }


        private List<NewsRecommendedQuestionViewModel> TryGetNewsRecommendedQuestions(int userId, int questionsPage = 1, int questionsAmount = 100)
        {

            if (!NewsQuestionsWrapper.TryGetNewsRecommendedQuestions(_context, userId, questionsPage, questionsAmount,
                out var newsRecommendedQuestions))
                return new List<NewsRecommendedQuestionViewModel>();

            var result = new List<NewsRecommendedQuestionViewModel>();
            int index = 0;
            Parallel.ForEach(newsRecommendedQuestions, (question) =>
            {
                GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
                var resultQuestion = question.MapNewsRecommendedQuestionsViewModel(tags, firstPhotos, secondPhotos);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            });
            return result;
        }

        private void GetTagsAndPhotos(int userId, int questionId, out IEnumerable<TagDb> tags,
            out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos)
        {
            var photosAmount = 100;
            var minAge = 0;
            var maxAge = 100;
            var sex = 0;
            var city = string.Empty;

            if (!TagsWrapper.TryGetTags(_context, questionId, out tags))
                tags = new List<TagDb>();
            if (!ResultsWrapper.TryGetPhotos(_context, userId, questionId, 1, photosAmount, minAge, maxAge, sex, city, out firstPhotos))
                firstPhotos = new List<PhotoDb>();
            if (!ResultsWrapper.TryGetPhotos(_context, userId, questionId, 2, photosAmount, minAge, maxAge, sex, city, out secondPhotos))
                secondPhotos = new List<PhotoDb>();
        }
    }
}
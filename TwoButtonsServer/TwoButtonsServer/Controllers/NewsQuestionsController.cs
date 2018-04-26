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
            if (newsViewModel == null)
                return BadRequest($"Input parameter  is null");

            int userId= newsViewModel.UserId;
            int questionsAmount = newsViewModel.QuestionsAmount;

            var askedList = Task.Run(()=>GetNewsAskedQuestions(userId, questionsAmount)).GetAwaiter().GetResult();
            var answeredList = Task.Run(() => GetNewsAnsweredQuestions(userId, questionsAmount)).GetAwaiter().GetResult();
            var favoriteList = Task.Run(() => GetNewsFavoriteQuestions(userId, questionsAmount)).GetAwaiter().GetResult();
            var commentedList = Task.Run(() => GetNewsCommentedQuestions(userId, questionsAmount)).GetAwaiter().GetResult();
            var recommentedList = Task.Run(() => TryGetNewsRecommendedQuestions(userId, questionsAmount)).GetAwaiter().GetResult();

         

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

        private List<NewsAskedQuestionViewModel> GetNewsAskedQuestions(int userId, int questionsAmount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsAskedQuestions(_context, userId, questionsAmount, out var userAskedQuestions))
                return new List<NewsAskedQuestionViewModel>();

            var result = new List<NewsAskedQuestionViewModel>();

            int index = 0;

            Parallel.ForEach(userAskedQuestions, (x) =>
            {
                GetTagsAndPhotos(userId, x.QuestionId, out var tags, out var firstPhotos, out var secondPhotos, out var comments);
                var resultQuestion = x.MapToNewsAskedQuestionsViewModel(tags, firstPhotos, secondPhotos, comments);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            });

            //foreach (var question in userAskedQuestions)
            //{
                

            //}
            return result;

        }


        private List<NewsAnsweredQuestionViewModel> GetNewsAnsweredQuestions(int userId, int questionsAmount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsAnsweredQuestions(_context, userId, questionsAmount,
                out var userAnsweredQuestions))
                return new List<NewsAnsweredQuestionViewModel>();

            var result = new List<NewsAnsweredQuestionViewModel>();
            int index = 0;
            Parallel.ForEach(userAnsweredQuestions, (question) =>
            {
                GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos, out var comments);
                var resultQuestion = question.MapToNewsAnsweredQuestionsViewModel(tags, firstPhotos, secondPhotos, comments);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            });
            return result;
        }


        private List<NewsFavoriteQuestionViewModel> GetNewsFavoriteQuestions(int userId, int questionsAmount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsFavoriteQuestions(_context, userId, questionsAmount,
                out var userFavoriteQuestions))
                return new List<NewsFavoriteQuestionViewModel>();

            var result = new List<NewsFavoriteQuestionViewModel>();
            int index = 0;
            Parallel.ForEach(userFavoriteQuestions, (question) =>
            {
                GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos, out var comments);
                var resultQuestion = question.MapToNewsFavoriteQuestionsViewModel(tags, firstPhotos, secondPhotos, comments);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            });
            return result;
        }


        private List<NewsCommentedQuestionViewModel> GetNewsCommentedQuestions( int userId, int questionsAmount = 100)
        {
            if (!NewsQuestionsWrapper.TryGetNewsCommentedQuestions(_context, userId, questionsAmount,
                out var userCommentedQuestions))
                return new List<NewsCommentedQuestionViewModel>();

            var result = new List<NewsCommentedQuestionViewModel>();
            int index = 0;
            Parallel.ForEach(userCommentedQuestions, (question) =>
            {
                GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos, out var comments);
                var resultQuestion = question.MapToNewsCommentedQuestionsViewModel(tags, firstPhotos, secondPhotos, comments);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            });
            return result;
        }


        private List<NewsRecommendedQuestionViewModel> TryGetNewsRecommendedQuestions(int userId, int questionsAmount = 100)
        {

            if (!NewsQuestionsWrapper.TryGetNewsRecommendedQuestions(_context, userId, questionsAmount,
                out var newsRecommendedQuestions))
                return new List<NewsRecommendedQuestionViewModel>();

            var result = new List<NewsRecommendedQuestionViewModel>();
            int index = 0;
            Parallel.ForEach(newsRecommendedQuestions, (question) =>
            {
                GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos, out var comments);
                var resultQuestion = question.MapNewsRecommendedQuestionsViewModel(tags, firstPhotos, secondPhotos, comments);
                resultQuestion.IndexNumber = index++;
                result.Add(resultQuestion);
            });
            return result;
        }

        private void GetTagsAndPhotos(int userId, int questionId, out IEnumerable<TagDb> tags,
            out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos, out IEnumerable<CommentDb> comments)
        {
            var commentsAmount = 100;
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
            if (!CommentsWrapper.TryGetComments(_context, userId, questionId, commentsAmount, out  comments))
                comments = new List<CommentDb>();
        }
    }
}
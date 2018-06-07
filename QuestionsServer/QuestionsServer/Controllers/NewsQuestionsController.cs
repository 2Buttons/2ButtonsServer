using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.Helpers;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuestionsData;
using QuestionsData.Entities;
using QuestionsData.Queries;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;
using QuestionsServer.ViewModels.OutputParameters;
using QuestionsServer.ViewModels.OutputParameters.NewsQuestions;

namespace QuestionsServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("api/[controller]")]
  [Route("news")]
  public class NewsQuestionsController : Controller //Don't receive deleted
  {
    private readonly QuestionsUnitOfWork _mainDb;

    public NewsQuestionsController(QuestionsUnitOfWork mainDb)
    {
      _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<IActionResult> GetNews([FromBody] GetNewsViewModel newsViewModel)
    {
      //TODO нормально вычислить сколько брать в бд. Возможно стоит сразу брать в процентах

      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var userId = newsViewModel.UserId;
      var questionsPage = newsViewModel.PageParams.Offset / 5;
      var questionsAmount = newsViewModel.PageParams.Count / 5;

      var askedListTask =  GetNewsAskedQuestionsAsync(userId, questionsPage, questionsAmount);
      var answeredListTask =  GetNewsAnsweredQuestionsAsync(userId, questionsPage, questionsAmount);
      var favoriteListTask =  GetNewsFavoriteQuestionsAsync(userId, questionsPage, questionsAmount);
      var commentedListTask =  GetNewsCommentedQuestions(userId, questionsPage, questionsAmount);
      var recommentedListTask =  TryGetNewsRecommendedQuestions(userId, questionsPage, questionsAmount);

      await Task.WhenAll(askedListTask, answeredListTask, favoriteListTask, commentedListTask, recommentedListTask);

      var askedList = askedListTask.Result;
      var answeredList = answeredListTask.Result;
      var favoriteList = favoriteListTask.Result;
      var commentedList = commentedListTask.Result;
      var recommentedList = recommentedListTask.Result;

      var answeredListCount = newsViewModel.PageParams.Count / 100 * 16;
      var favoriteListCount = newsViewModel.PageParams.Count / 100 * 16;
      var commentedListCount = newsViewModel.PageParams.Count / 100 * 19;
      var recommentedListCount = newsViewModel.PageParams.Count / 100 * 18;
      var askedListCount = newsViewModel.PageParams.Count - answeredListCount - favoriteListCount -
                           commentedListCount - recommentedListCount;

      var answeredListresultList = answeredList.Take(answeredListCount).ToList();
      var favoriteListresultList = favoriteList.Take(favoriteListCount).ToList();
      var commentedListresultList = commentedList.Take(commentedListCount).ToList();
      var recommentedListresultList = recommentedList.Take(recommentedListCount).ToList();
      var askedListresultList = askedList.Take(askedListCount).ToList();

      var mainList = new List<NewsQuestionBaseViewModel>(answeredListCount + favoriteListCount + commentedListCount +
                                                         recommentedListCount + askedListCount);

      mainList.AddRange(answeredListresultList);
      mainList.AddRange(favoriteListresultList);
      mainList.AddRange(commentedListresultList);
      mainList.AddRange(recommentedListresultList);
      mainList.AddRange(askedListresultList);

      var resultList = mainList.OrderByDescending(x => x.Priority).ToList();

      for (var i = 0; i < resultList.Count; i++)
        mainList[i].Position = i;

      // await Task.WhenAll(new Task[] { askedList, answeredList, favoriteList, commentedList, recommentedList});

      var result = new NewsViewModel
      {
        NewsAskedQuestions = askedListresultList,
        NewsAnsweredQuestions = answeredListresultList,
        NewsFavoriteQuestions = favoriteListresultList,
        NewsCommentedQuestions = commentedListresultList,
        NewsRecommendedQuestions = recommentedListresultList
      };

      MonitoringServerHelper.UpdateUrlMonitoring(newsViewModel.UserId, UrlMonitoringType.GetsQuestionsNews);
      return new OkResponseResult(result);
    }

    private async Task<List<NewsAskedQuestionViewModel>> GetNewsAskedQuestionsAsync(int userId, int questionsPage = 1,
      int questionsAmount = 10)
    {
      var userAskedQuestions = await _mainDb.News.GetNewsAskedQuestions(userId, questionsPage, questionsAmount);
        //return new List<NewsAskedQuestionViewModel>();

      var result = new List<NewsAskedQuestionViewModel>();

      foreach (var x in userAskedQuestions)
      {
        GetTagsAndPhotos(userId, x.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = x.MapToNewsAskedQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      }
      return result;
    }


    private async Task<List<NewsAnsweredQuestionViewModel>> GetNewsAnsweredQuestionsAsync(int userId, int questionsPage = 1,
      int questionsAmount = 10)
    {
     var userAnsweredQuestions = await _mainDb.News.GetNewsAnsweredQuestions(userId, questionsPage, questionsAmount);
      //return new List<NewsAnsweredQuestionViewModel>();
      var result = new List<NewsAnsweredQuestionViewModel>();
      foreach (var question in userAnsweredQuestions)
      {
        GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapToNewsAnsweredQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      }
      return result;
    }


    private async Task<List<NewsFavoriteQuestionViewModel>> GetNewsFavoriteQuestionsAsync(int userId, int questionsPage = 1,
      int questionsAmount = 10)
    {
      var userFavoriteQuestions  = await _mainDb.News.GetNewsFavoriteQuestions(userId, questionsPage, questionsAmount);
        //return new List<NewsFavoriteQuestionViewModel>();

      var result = new List<NewsFavoriteQuestionViewModel>();

      foreach (var question in userFavoriteQuestions)
      {
        GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapToNewsFavoriteQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      }
      return result;
    }


    private async Task<List<NewsCommentedQuestionViewModel>> GetNewsCommentedQuestions(int userId, int questionsPage = 1,
      int questionsAmount = 10)
    {
      var userCommentedQuestions = await _mainDb.News.GetNewsCommentedQuestions(userId, questionsPage, questionsAmount);
        //return new List<NewsCommentedQuestionViewModel>();

      var result = new List<NewsCommentedQuestionViewModel>();
      foreach (var question in userCommentedQuestions)
      {
        GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapToNewsCommentedQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      }
      return result;
    }


    private async  Task<List<NewsRecommendedQuestionViewModel>> TryGetNewsRecommendedQuestions(int userId, int questionsPage = 1,
      int questionsAmount = 10)
    {
      var newsRecommendedQuestions =
        await _mainDb.News.GetNewsRecommendedQuestions(userId, questionsPage, questionsAmount);

      var result = new List<NewsRecommendedQuestionViewModel>();

      foreach (var question in newsRecommendedQuestions)
      {
        GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapNewsRecommendedQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      }
     
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

      //var tagsTask = _mainDb.Tags.GetTags(questionId);
      //var firstPhotosTask = _mainDb.Questions.GetPhotos(userId, questionId, 1, photosAmount, maxAge.WhenBorned(),
      //  minAge.WhenBorned(), sex, city);
      //var secondPhotosTask = _mainDb.Questions.GetPhotos(userId, questionId, 2, photosAmount, maxAge.WhenBorned(),
      //  minAge.WhenBorned(), sex, city);
      //Task.WhenAll(tagsTask, firstPhotosTask, secondPhotosTask);
      //tags = tagsTask.Result;
      //firstPhotos = firstPhotosTask.Result;
      //secondPhotos = secondPhotosTask.Result;


      tags = _mainDb.Tags.GetTags(questionId).GetAwaiter().GetResult();
      firstPhotos = _mainDb.Questions.GetPhotos(userId, questionId, 1, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
      secondPhotos =  _mainDb.Questions.GetPhotos(userId, questionId, 2, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city).GetAwaiter().GetResult();
    }
  }
}
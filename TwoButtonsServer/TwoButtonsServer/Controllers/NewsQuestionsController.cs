using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoButtonsDatabase;
using TwoButtonsDatabase.Entities;
using TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels;
using TwoButtonsServer.ViewModels.OutputParameters;
using TwoButtonsServer.ViewModels.OutputParameters.NewsQuestions;

namespace TwoButtonsServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("api/[controller]")]
  [Route("news")]
  public class NewsQuestionsController : Controller //Don't receive deleted
  {
    private readonly TwoButtonsUnitOfWork _uowMain;

    public NewsQuestionsController(TwoButtonsUnitOfWork uowMain)
    {
      _uowMain = uowMain;
    }

    [HttpPost]
    public async Task<IActionResult> GetNews([FromBody] GetNewsViewModel newsViewModel)
    {
      //TODO нормально вычислить сколько брать в бд. Возможно стоит сразу брать в процентах

      if (newsViewModel?.PageParams == null)
        return BadRequest($"Input parameter  is null");

      var userId = newsViewModel.UserId;
      var questionsPage = newsViewModel.PageParams.Offset / 5;
      var questionsAmount = newsViewModel.PageParams.Count / 5;

      var askedList = Task.Run(() => GetNewsAskedQuestions(userId, questionsPage, questionsAmount)).GetAwaiter()
        .GetResult();
      var answeredList = Task.Run(() => GetNewsAnsweredQuestions(userId, questionsPage, questionsAmount)).GetAwaiter()
        .GetResult();
      var favoriteList = Task.Run(() => GetNewsFavoriteQuestions(userId, questionsPage, questionsAmount)).GetAwaiter()
        .GetResult();
      var commentedList = Task.Run(() => GetNewsCommentedQuestions(userId, questionsPage, questionsAmount)).GetAwaiter()
        .GetResult();
      var recommentedList = Task.Run(() => TryGetNewsRecommendedQuestions(userId, questionsPage, questionsAmount))
        .GetAwaiter().GetResult();


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


      return Ok(result);
    }

    private List<NewsAskedQuestionViewModel> GetNewsAskedQuestions(int userId, int questionsPage = 1,
      int questionsAmount = 100)
    {
      if (!_uowMain.News.TryGetNewsAskedQuestions(userId, questionsPage, questionsAmount, out var userAskedQuestions))
        return new List<NewsAskedQuestionViewModel>();

      var result = new List<NewsAskedQuestionViewModel>();

      Parallel.ForEach(userAskedQuestions, x =>
      {
        GetTagsAndPhotos(userId, x.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = x.MapToNewsAskedQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      });
      return result;
    }


    private List<NewsAnsweredQuestionViewModel> GetNewsAnsweredQuestions(int userId, int questionsPage = 1,
      int questionsAmount = 100)
    {
      if (!_uowMain.News.TryGetNewsAnsweredQuestions(userId, questionsPage, questionsAmount,
        out var userAnsweredQuestions))
        return new List<NewsAnsweredQuestionViewModel>();

      var result = new List<NewsAnsweredQuestionViewModel>();
      var index = 0;
      Parallel.ForEach(userAnsweredQuestions, question =>
      {
        GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapToNewsAnsweredQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      });
      return result;
    }


    private List<NewsFavoriteQuestionViewModel> GetNewsFavoriteQuestions(int userId, int questionsPage = 1,
      int questionsAmount = 100)
    {
      if (!_uowMain.News.TryGetNewsFavoriteQuestions(userId, questionsPage, questionsAmount,
        out var userFavoriteQuestions))
        return new List<NewsFavoriteQuestionViewModel>();

      var result = new List<NewsFavoriteQuestionViewModel>();
      var index = 0;
      Parallel.ForEach(userFavoriteQuestions, question =>
      {
        GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapToNewsFavoriteQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      });
      return result;
    }


    private List<NewsCommentedQuestionViewModel> GetNewsCommentedQuestions(int userId, int questionsPage = 1,
      int questionsAmount = 100)
    {
      if (!_uowMain.News.TryGetNewsCommentedQuestions(userId, questionsPage, questionsAmount,
        out var userCommentedQuestions))
        return new List<NewsCommentedQuestionViewModel>();

      var result = new List<NewsCommentedQuestionViewModel>();
      Parallel.ForEach(userCommentedQuestions, question =>
      {
        GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapToNewsCommentedQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      });
      return result;
    }


    private List<NewsRecommendedQuestionViewModel> TryGetNewsRecommendedQuestions(int userId, int questionsPage = 1,
      int questionsAmount = 100)
    {
      if (!_uowMain.News.TryGetNewsRecommendedQuestions(userId, questionsPage, questionsAmount,
        out var newsRecommendedQuestions))
        return new List<NewsRecommendedQuestionViewModel>();

      var result = new List<NewsRecommendedQuestionViewModel>();
      Parallel.ForEach(newsRecommendedQuestions, question =>
      {
        GetTagsAndPhotos(userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapNewsRecommendedQuestionsViewModel(tags, firstPhotos, secondPhotos);
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

      if (!_uowMain.Tags.TryGetTags(questionId, out tags))
        tags = new List<TagDb>();
      if (!_uowMain.Questions.TryGetPhotos(userId, questionId, 1, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city, out firstPhotos))
        firstPhotos = new List<PhotoDb>();
      if (!_uowMain.Questions.TryGetPhotos(userId, questionId, 2, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city, out secondPhotos))
        secondPhotos = new List<PhotoDb>();
    }
  }
}
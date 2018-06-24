using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Extensions;
using CommonLibraries.Helpers;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestionsData;
using QuestionsData.Entities;
using QuestionsData.Queries;
using QuestionsServer.Infrastructure;
using QuestionsServer.ViewModels.InputParameters.ControllersViewModels;
using QuestionsServer.ViewModels.OutputParameters;
using QuestionsServer.ViewModels.OutputParameters.NewsQuestions;


namespace QuestionsServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  //[Route("api/[controller]")]
  [Route("questions")]
  public class NewsQuestionsController : Controller //Don't receive deleted
  {
    private readonly QuestionsUnitOfWork _mainDb;
    private readonly DbContextOptions<TwoButtonsContext> _dbOptions;
    private readonly ConnectionsHub _hub;

    public NewsQuestionsController(QuestionsUnitOfWork mainDb, DbContextOptions<TwoButtonsContext> dbOptions, ConnectionsHub hub)
    {
      _mainDb = mainDb;
      _dbOptions = dbOptions;
      _hub = hub;
    }


    [HttpPost("news")]
    public async Task<IActionResult> GetNews([FromBody] GetNewsViewModel newsVm)
    {

      //TODO нормально вычислить сколько брать в бд. Возможно стоит сразу брать в процентах

      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      var userId = newsVm.UserId;
      var questionsOffset = newsVm.PageParams.Offset;
      var questionsCount = newsVm.PageParams.Count;

      var askedList = new List<NewsAskedQuestionViewModel>();
      var answeredList = new List<NewsAnsweredQuestionViewModel>();
      var favoriteList = new List<NewsFavoriteQuestionViewModel>();
      var commentedList = new List<NewsCommentedQuestionViewModel>();
      var recommentedList = new List<NewsRecommendedQuestionViewModel>();

      Parallel.Invoke(
        () =>
        {
          using (var context = new TwoButtonsContext(_dbOptions))
            askedList = GetNewsAskedQuestionsAsync(context, userId, questionsOffset, questionsCount);
        },
        () =>
        {
          using (var context = new TwoButtonsContext(_dbOptions))
            answeredList = GetNewsAnsweredQuestionsAsync(context, userId, questionsOffset, questionsCount);
        },
        () =>
        {
          using (var context = new TwoButtonsContext(_dbOptions))
            favoriteList = GetNewsFavoriteQuestionsAsync(context, userId, questionsOffset, questionsCount);
        },
        () =>
        {
          using (var context = new TwoButtonsContext(_dbOptions))
            commentedList = GetNewsCommentedQuestions(context, userId, questionsOffset, questionsCount);
        },
        () =>
        {
          using (var context = new TwoButtonsContext(_dbOptions))
            recommentedList = TryGetNewsRecommendedQuestions(context, userId, questionsOffset, questionsCount);
        });


      var newsList = new List<NewsQuestionBaseViewModel>(askedList.Count + answeredList.Count + commentedList.Count +
                                                         favoriteList.Count + recommentedList.Count);

      newsList.AddRange(askedList);
      newsList.AddRange(answeredList);
      newsList.AddRange(commentedList);
      newsList.AddRange(favoriteList);
      newsList.AddRange(recommentedList);

      var mainList = newsList.OrderBy(x => x.UserId).ToList();

      var removeList = new List<NewsQuestionBaseViewModel>();
      var compare = new NewsQuestionBaseCompare();
      for (var i = 0; i < mainList.Count-1; i++)
      {
        if (compare.GetHashCode(mainList[i]) == compare.GetHashCode(mainList[i + 1]) ||
            compare.Equals(mainList[i], mainList[i + 1]))
        {
          removeList.Add(mainList[i].Priority > mainList[i + 1].Priority ? mainList[i + 1] : mainList[i]);
        }
      }

      var resultList = mainList.Except(removeList).OrderByDescending(x => x.Priority).ToList();

      var answeredListResultList = new List<NewsAnsweredQuestionViewModel>();
      var favoriteListResultList = new List<NewsFavoriteQuestionViewModel>();
      var commentedListResultList = new List<NewsCommentedQuestionViewModel>();
      var recommentedListResultList = new List<NewsRecommendedQuestionViewModel>();

      var askedListResultList = new List<NewsAskedQuestionViewModel>();


      var length = questionsCount > resultList.Count ? resultList.Count : questionsCount;

      for (var i = 0; i < length; i++)
      {
        resultList[i].Position = i + questionsOffset;
        switch (resultList[i].Type)
        {
          case NewsType.Answered:
            answeredListResultList.Add((NewsAnsweredQuestionViewModel)resultList[i]);
            break;
          case NewsType.Asked:
            askedListResultList.Add((NewsAskedQuestionViewModel)resultList[i]);
            break;
          case NewsType.Commented:
            commentedListResultList.Add((NewsCommentedQuestionViewModel)resultList[i]);
            break;
          case NewsType.Favorite:
            favoriteListResultList.Add((NewsFavoriteQuestionViewModel)resultList[i]);
            break;
          case NewsType.Recommended:
            recommentedListResultList.Add((NewsRecommendedQuestionViewModel)resultList[i]);
            break;
        }
      }
      //if (answeredListCount == 0 && answeredList.Count > 0) answeredListCount = 1;
      //if (favoriteListCount == 0 && favoriteList.Count > 0) answeredListCount = 1;
      //if (commentedListCount == 0 && commentedList.Count > 0) answeredListCount = 1;
      //if (recommentedListCount == 0 && recommentedList.Count > 0) answeredListCount=1;
      //if (askedListCount == 0 && askedList.Count > 0) answeredListCount = 1;

      //List<(int count, NewsType type)> counts = new List<(int, NewsType)>()
      //{
      //  (answeredListCount,NewsType.Answered),
      //  (favoriteListCount,NewsType.Favorite),
      //  (commentedListCount,NewsType.Commented),
      //  (recommentedListCount,NewsType.Recommended),
      //  (askedListCount,NewsType.Asked)
      //};
    

      //List<NewsAnsweredQuestionViewModel> answeredListResultList = answeredList.Take(answeredListCount).ToList();
      //List<NewsFavoriteQuestionViewModel> favoriteListResultList = favoriteList.Take(favoriteListCount).ToList();
      //List<NewsCommentedQuestionViewModel> commentedListResultList = commentedList.Take(commentedListCount).ToList();
      //List<NewsRecommendedQuestionViewModel> recommentedListResultList = recommentedList.Take(recommentedListCount).ToList();
      //List<NewsAskedQuestionViewModel> askedListResultList = askedList.Take(askedListCount).ToList();

      var result = new NewsViewModel
      {
        NewsAskedQuestions = askedListResultList,
        NewsAnsweredQuestions = answeredListResultList,
        NewsFavoriteQuestions = favoriteListResultList,
        NewsCommentedQuestions = commentedListResultList,
        NewsRecommendedQuestions = recommentedListResultList
      };

      _hub.Monitoring.UpdateUrlMonitoring(newsVm.UserId, UrlMonitoringType.GetsQuestionsNews);

      return new OkResponseResult(result);
    }

    private List<NewsAskedQuestionViewModel> GetNewsAskedQuestionsAsync(TwoButtonsContext context, int userId, int questionsPage = 1,
      int questionsAmount = 10)
    {
      var userAskedQuestions = _mainDb.News.GetNewsAskedQuestions(context, userId, questionsPage, questionsAmount);
      //return new List<NewsAskedQuestionViewModel>();

      var result = new List<NewsAskedQuestionViewModel>();

      foreach (var question in userAskedQuestions)
      {
        GetTagsAndPhotos(context, userId, question.NewsAskedQuestionDb.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapToNewsAskedQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      }
      return result;
    }


    private List<NewsAnsweredQuestionViewModel> GetNewsAnsweredQuestionsAsync(TwoButtonsContext context, int userId, int questionsPage = 1,
      int questionsAmount = 10)
    {
      var userAnsweredQuestions = _mainDb.News.GetNewsAnsweredQuestions(context, userId, questionsPage, questionsAmount);
      //return new List<NewsAnsweredQuestionViewModel>();
      var result = new List<NewsAnsweredQuestionViewModel>();
      foreach (var question in userAnsweredQuestions)
      {
        GetTagsAndPhotos(context, userId, question.NewsAnsweredQuestionDb.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapToNewsAnsweredQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      }
      return result;
    }


    private List<NewsFavoriteQuestionViewModel> GetNewsFavoriteQuestionsAsync(TwoButtonsContext context, int userId, int questionsPage = 1,
      int questionsAmount = 10)
    {
      var userFavoriteQuestions = _mainDb.News.GetNewsFavoriteQuestions(context, userId, questionsPage, questionsAmount);
      //return new List<NewsFavoriteQuestionViewModel>();

      var result = new List<NewsFavoriteQuestionViewModel>();

      foreach (var question in userFavoriteQuestions)
      {
        GetTagsAndPhotos(context, userId, question.NewsFavoriteQuestionDb.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapToNewsFavoriteQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      }
      return result;
    }


    private List<NewsCommentedQuestionViewModel> GetNewsCommentedQuestions(TwoButtonsContext context, int userId, int questionsPage = 1,
      int questionsAmount = 10)
    {
      var userCommentedQuestions = _mainDb.News.GetNewsCommentedQuestions(context, userId, questionsPage, questionsAmount);
      //return new List<NewsCommentedQuestionViewModel>();

      var result = new List<NewsCommentedQuestionViewModel>();
      foreach (var question in userCommentedQuestions)
      {
        GetTagsAndPhotos(context, userId, question.NewsCommentedQuestionDb.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapToNewsCommentedQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      }
      return result;
    }


    private List<NewsRecommendedQuestionViewModel> TryGetNewsRecommendedQuestions(TwoButtonsContext context, int userId, int questionsPage = 1,
      int questionsAmount = 10)
    {
      var newsRecommendedQuestions =
         _mainDb.News.GetNewsRecommendedQuestions(context, userId, questionsPage, questionsAmount);

      var result = new List<NewsRecommendedQuestionViewModel>();

      foreach (var question in newsRecommendedQuestions)
      {
        GetTagsAndPhotos(context, userId, question.NewsRecommendedQuestionDb.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion = question.MapNewsRecommendedQuestionsViewModel(tags, firstPhotos, secondPhotos);
        result.Add(resultQuestion);
      }

      return result;
    }


    private void GetTagsAndPhotos(TwoButtonsContext context, int userId, int questionId, out IEnumerable<TagDb> tags,
      out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos)
    {
      var photosAmount = 100;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      tags = _mainDb.Tags.GetTags(context, questionId);
      firstPhotos = _mainDb.Questions.GetPhotos(context, userId, questionId, 1, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city);
      secondPhotos = _mainDb.Questions.GetPhotos(context, userId, questionId, 2, photosAmount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city);
    }

  }
}
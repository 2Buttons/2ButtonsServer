using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.ConnectionServices;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestionsData;
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
    private QuestionsUnitOfWork MainDb { get; }
    private DbContextOptions<TwoButtonsContext> DbOptions { get; }
    private ConnectionsHub Hub { get; }
    private ILogger<NewsQuestionsController> Logger { get; }
    private MediaConverter MediaConverter { get; }

    public NewsQuestionsController(QuestionsUnitOfWork mainDb, DbContextOptions<TwoButtonsContext> dbOptions,
      ConnectionsHub hub, ILogger<NewsQuestionsController> logger, MediaConverter mediaConverter)
    {
      MainDb = mainDb;
      DbOptions = dbOptions;
      Hub = hub;
      Logger = logger;
      MediaConverter = mediaConverter;
    }

    [HttpPost("news")]
    public async Task<IActionResult> GetNews([FromBody] GetNewsViewModel newsVm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);

      Logger.LogInformation($"{nameof(NewsQuestionsController)}.{nameof(GetNews)}.Start");
      var userId = newsVm.UserId;
      var questionsOffset = newsVm.PageParams.Offset;
      var questionsCount = newsVm.PageParams.Count;

      var askedList = new List<NewsAskedQuestionViewModel>();
      var answeredList = new List<NewsAnsweredQuestionViewModel>();
      var favoriteList = new List<NewsFavoriteQuestionViewModel>();
      var commentedList = new List<NewsCommentedQuestionViewModel>();
      var recommentedList = new List<NewsRecommendedQuestionViewModel>();

      Parallel.Invoke(() =>
      {
        using (var context = new TwoButtonsContext(DbOptions))
        {
          askedList = GetNewsAskedQuestionsAsync(context, userId, newsVm.BackgroundSizeType);
        }
      }, () =>
      {
        using (var context = new TwoButtonsContext(DbOptions))
        {
          answeredList = GetNewsAnsweredQuestionsAsync(context, userId, newsVm.BackgroundSizeType);
        }
      }, () =>
      {
        using (var context = new TwoButtonsContext(DbOptions))
        {
          favoriteList = GetNewsFavoriteQuestionsAsync(context, userId, newsVm.BackgroundSizeType);
        }
      }, () =>
      {
        using (var context = new TwoButtonsContext(DbOptions))
        {
          commentedList = GetNewsCommentedQuestions(context, userId, newsVm.BackgroundSizeType);
        }
      }, () =>
      {
        using (var context = new TwoButtonsContext(DbOptions))
        {
          recommentedList = TryGetNewsRecommendedQuestions(context, userId, newsVm.BackgroundSizeType);
        }
      });

      var newsList = new List<NewsQuestionBaseViewModel>(askedList.Count + answeredList.Count + commentedList.Count +
                                                         favoriteList.Count + recommentedList.Count);

      newsList.AddRange(askedList);
      newsList.AddRange(answeredList);
      newsList.AddRange(commentedList);
      newsList.AddRange(favoriteList);
      newsList.AddRange(recommentedList);

      var mainList = newsList.OrderBy(x => x.Author.UserId).ToList();

      var removeList = new List<NewsQuestionBaseViewModel>();
      var compare = new NewsQuestionBaseCompare();
      for (var i = 0; i < mainList.Count - 1; i++)
        if (compare.GetHashCode(mainList[i]) == compare.GetHashCode(mainList[i + 1]) ||
            compare.Equals(mainList[i], mainList[i + 1]))
          removeList.Add(mainList[i].Priority > mainList[i + 1].Priority ? mainList[i + 1] : mainList[i]);

      var resultList = mainList.Except(removeList).OrderByDescending(x => x.Priority).Skip(questionsOffset)
        .Take(questionsCount).ToList();

      var answeredListResultList = new List<NewsAnsweredQuestionViewModel>();
      var favoriteListResultList = new List<NewsFavoriteQuestionViewModel>();
      var commentedListResultList = new List<NewsCommentedQuestionViewModel>();
      var recommentedListResultList = new List<NewsRecommendedQuestionViewModel>();

      var askedListResultList = new List<NewsAskedQuestionViewModel>();

      var length = questionsCount > resultList.Count ? resultList.Count : questionsCount;

      for (var i = 0; i < length; i++)
      {
        resultList[i].Position = i + questionsOffset;
        switch (resultList[i].NewsType)
        {
          case NewsType.Answered:
            answeredListResultList.Add((NewsAnsweredQuestionViewModel) resultList[i]);
            break;
          case NewsType.Asked:
            askedListResultList.Add((NewsAskedQuestionViewModel) resultList[i]);
            break;
          case NewsType.Commented:
            commentedListResultList.Add((NewsCommentedQuestionViewModel) resultList[i]);
            break;
          case NewsType.Favorite:
            favoriteListResultList.Add((NewsFavoriteQuestionViewModel) resultList[i]);
            break;
          case NewsType.Recommended:
            recommentedListResultList.Add((NewsRecommendedQuestionViewModel) resultList[i]);
            break;
        }
      }

      var result = new NewsViewModel
      {
        NewsAskedQuestions = askedListResultList,
        NewsAnsweredQuestions = answeredListResultList,
        NewsFavoriteQuestions = favoriteListResultList,
        NewsCommentedQuestions = commentedListResultList,
        NewsRecommendedQuestions = recommentedListResultList
      };

      await Hub.Monitoring.UpdateUrlMonitoring(newsVm.UserId, UrlMonitoringType.GetsQuestionsNews);
      Logger.LogInformation($"{nameof(NewsQuestionsController)}.{nameof(GetNews)}.End");
      return new OkResponseResult(result);
    }

    private List<NewsAskedQuestionViewModel> GetNewsAskedQuestionsAsync(TwoButtonsContext context, int userId,
      BackgroundSizeType backgroundSizeType)
    {
      var userAskedQuestions = MainDb.News.GetNewsAskedQuestions(context, userId);
      //return new List<NewsAskedQuestionViewModel>();

      var result = new List<NewsAskedQuestionViewModel>();

      foreach (var question in userAskedQuestions)
      {
        GetTagsAndPhotos(context, userId, question.NewsAskedQuestionDb.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        var resultQuestion =
          question.MapToNewsAskedQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
            backgroundSizeType);
        result.Add(resultQuestion);
      }
      return result;
    }

    private List<NewsAnsweredQuestionViewModel> GetNewsAnsweredQuestionsAsync(TwoButtonsContext context, int userId,
      BackgroundSizeType backgroundSizeType)
    {
      var userAnsweredQuestions = MainDb.News.GetNewsAnsweredQuestions(context, userId);
      //return new List<NewsAnsweredQuestionViewModel>();
      var result = new List<NewsAnsweredQuestionViewModel>();
      foreach (var question in userAnsweredQuestions)
      {
        GetTagsAndPhotos(context, userId, question.NewsAnsweredQuestionDb.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        var resultQuestion =
          question.MapToNewsAnsweredQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
            backgroundSizeType);
        result.Add(resultQuestion);
      }
      return result;
    }

    private List<NewsFavoriteQuestionViewModel> GetNewsFavoriteQuestionsAsync(TwoButtonsContext context, int userId,
      BackgroundSizeType backgroundSizeType)
    {
      var userFavoriteQuestions = MainDb.News.GetNewsFavoriteQuestions(context, userId);
      //return new List<NewsFavoriteQuestionViewModel>();

      var result = new List<NewsFavoriteQuestionViewModel>();

      foreach (var question in userFavoriteQuestions)
      {
        GetTagsAndPhotos(context, userId, question.NewsFavoriteQuestionDb.QuestionId, out var tags, out var firstPhotos,
          out var secondPhotos);
        var resultQuestion =
          question.MapToNewsFavoriteQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
            backgroundSizeType);
        result.Add(resultQuestion);
      }
      return result;
    }

    private List<NewsCommentedQuestionViewModel> GetNewsCommentedQuestions(TwoButtonsContext context, int userId,
      BackgroundSizeType backgroundSizeType)
    {
      var userCommentedQuestions = MainDb.News.GetNewsCommentedQuestions(context, userId);
      //return new List<NewsCommentedQuestionViewModel>();

      var result = new List<NewsCommentedQuestionViewModel>();
      foreach (var question in userCommentedQuestions)
      {
        GetTagsAndPhotos(context, userId, question.NewsCommentedQuestionDb.QuestionId, out var tags,
          out var firstPhotos, out var secondPhotos);
        var resultQuestion =
          question.MapToNewsCommentedQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
            backgroundSizeType);
        result.Add(resultQuestion);
      }
      return result;
    }

    private List<NewsRecommendedQuestionViewModel> TryGetNewsRecommendedQuestions(TwoButtonsContext context, int userId,
      BackgroundSizeType backgroundSizeType)
    {
      var newsRecommendedQuestions = MainDb.News.GetNewsRecommendedQuestions(context, userId);

      var result = new List<NewsRecommendedQuestionViewModel>();

      foreach (var question in newsRecommendedQuestions)
      {
        GetTagsAndPhotos(context, userId, question.QuestionId, out var tags, out var firstPhotos, out var secondPhotos);
        var resultQuestion =
          question.MapNewsRecommendedQuestionsViewModel(MediaConverter, tags, firstPhotos, secondPhotos,
            backgroundSizeType);
        result.Add(resultQuestion);
      }

      return result;
    }

    private void GetTagsAndPhotos(TwoButtonsContext context, int userId, int questionId, out IEnumerable<TagDb> tags,
      out IEnumerable<PhotoDb> firstPhotos, out IEnumerable<PhotoDb> secondPhotos)
    {
      var photosCount = 100;
      var minAge = 0;
      var maxAge = 100;
      var sex = 0;
      var city = string.Empty;

      tags = MainDb.Tags.GetTags(context, questionId);
      firstPhotos = MainDb.Questions.GetPhotos(context, userId, questionId, 1, photosCount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city);
      secondPhotos = MainDb.Questions.GetPhotos(context, userId, questionId, 2, photosCount, maxAge.WhenBorned(),
        minAge.WhenBorned(), sex, city);
    }
  }
}
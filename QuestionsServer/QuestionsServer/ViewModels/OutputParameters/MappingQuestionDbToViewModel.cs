using System.Collections.Generic;
using System.Linq;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;
using QuestionsData.DTO;
using QuestionsData.DTO.NewsQuestions;
using QuestionsData.Queries;
using QuestionsData.Queries.UserQuestions;
using QuestionsServer.ViewModels.OutputParameters.NewsQuestions;
using QuestionsServer.ViewModels.OutputParameters.UserQuestions;

namespace QuestionsServer.ViewModels.OutputParameters
{
  public static class MappingQuestionDbToViewModel
  {
    public static NewsAnsweredQuestionViewModel MapToNewsAnsweredQuestionsViewModel(this NewsAnsweredQuestionDto dto,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<NewsAnsweredQuestionViewModel>(dto.NewsAnsweredQuestionDb, dbTags, dbFirstPhotos,
          dbSecondPhotos, backgroundSizeType);

      viewModel.AnsweredFollowingsCount = dto.NewsAnsweredQuestionDb.AnsweredFollowingsCount;
      viewModel.Priority = dto.Priority;
      viewModel.NewsType =  CommonLibraries.NewsType.Answered;
      return viewModel;
    }

    public static NewsAskedQuestionViewModel MapToNewsAskedQuestionsViewModel(this NewsAskedQuestionDto dto,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<NewsAskedQuestionViewModel>(dto.NewsAskedQuestionDb, dbTags, dbFirstPhotos,
          dbSecondPhotos, backgroundSizeType);

      viewModel.Priority = dto.Priority;
      viewModel.NewsType = CommonLibraries.NewsType.Asked;
      return viewModel;
    }

    public static NewsCommentedQuestionViewModel MapToNewsCommentedQuestionsViewModel(this NewsCommentedQuestionDto dto,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<NewsCommentedQuestionViewModel>(dto.NewsCommentedQuestionDb, dbTags, dbFirstPhotos,
          dbSecondPhotos, backgroundSizeType);

      viewModel.CommentedUser = new NewsUserViewModel
      {
        UserId = dto.NewsCommentedQuestionDb.CommentedUserId,
        Login = dto.NewsCommentedQuestionDb.CommentedUserLogin,
        SexType = dto.NewsCommentedQuestionDb.CommentedUserSexType
      };
      viewModel.UserCommentsCount = dto.NewsCommentedQuestionDb.CommentedUserCommentsCount;

      viewModel.Priority = dto.Priority;
      viewModel.NewsType = CommonLibraries.NewsType.Commented;

      return viewModel;
    }

    public static NewsFavoriteQuestionViewModel MapToNewsFavoriteQuestionsViewModel(this NewsFavoriteQuestionDto dto,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<NewsFavoriteQuestionViewModel>(dto.NewsFavoriteQuestionDb, dbTags, dbFirstPhotos,
          dbSecondPhotos, backgroundSizeType);
      viewModel.FavoriteAddedUser = new NewsUserViewModel
      {
        UserId = dto.NewsFavoriteQuestionDb.FavoriteAddedUserId,
        SexType = dto.NewsFavoriteQuestionDb.FavoriteAddedUserSexType,
        Login = dto.NewsFavoriteQuestionDb.FavoriteAddedUserLogin
      };

      viewModel.Priority = dto.Priority;
      viewModel.NewsType = CommonLibraries.NewsType.Favorite;
      return viewModel;
    }

    public static NewsRecommendedQuestionViewModel MapNewsRecommendedQuestionsViewModel(
      this NewsRecommendedQuestionDto dto, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
      IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<NewsRecommendedQuestionViewModel>(dto, dbTags, dbFirstPhotos,
          dbSecondPhotos, backgroundSizeType);

      viewModel.RecommendedUsers = new List<NewsUserViewModel>(dto.RecommendedUsers.Count);

      foreach (var user in dto.RecommendedUsers)
      {
        viewModel.RecommendedUsers.Add(new NewsUserViewModel
        {
          UserId = user.UserId,
          Login = user.Login,
          SexType = user.SexType
        });
      }



      viewModel.Priority = dto.Priority;
      viewModel.NewsType = CommonLibraries.NewsType.Recommended;

      return viewModel;
    }

    public static UserAnsweredQuestionsViewModel MapToUserAnsweredQuestionsViewModel(
      this UserAnsweredQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
      IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<UserAnsweredQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static UserAskedQuestionsViewModel MapToUserAskedQuestionsViewModel(this UserAskedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<UserAskedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static UserCommentedQuestionsViewModel MapToUserCommentedQuestionsViewModel(
      this UserCommentedQuestionDto dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
      IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<UserCommentedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);

      viewModel.Comments = dbEntity.Comments;
      return viewModel;
    }

    public static UserFavoriteQuestionsViewModel MapToUserFavoriteQuestionsViewModel(
      this UserFavoriteQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
      IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<UserFavoriteQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static TopQuestionsViewModel MapToTopQuestionsViewModel(this TopQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<TopQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static AskedQuestionsViewModel MapToAskedQuestionsViewModel(this AskedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<AskedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static SelectedQuestionsViewModel MapToChosenQuestionsViewModel(this SelectedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<SelectedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }


    public static RecommendedQuestionViewModel MapToRecommendedQuestionsViewModel(this RecommendedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<RecommendedQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      //viewModel.ToUserId = dbEntity.ToUserId;
      //viewModel.ToUserLogin = dbEntity.ToUserLogin;
      //viewModel.RecommendDate = dbEntity.RecommendDate;

      return viewModel;
    }

    public static RecommendedQuestionViewModel MapToRecommendedQuestionsViewModel(this RecommendedQuestionDto dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<RecommendedQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);

      viewModel.Users = dbEntity.RecommendedToUsers;
      return viewModel;
    }

    public static LikedQuestionsViewModel MapToLikedQuestionsViewModel(this LikedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<LikedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static SavedQuestionsViewModel MapToSavedQuestionsViewModel(this SavedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<SavedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static GetQuestionViewModel MapToGetQuestionsViewModel(this QuestionDb dbEntity, IEnumerable<TagDb> dbTags,
      IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> comments, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<GetQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      viewModel.Comments = comments.MapCommentsDbToViewModel();
      return viewModel;
    }

    public static T QuestionDbToViewModel<T>(this QuestionBaseDb dbEntity, IEnumerable<TagDb> dbTags,
      IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType) where T : QuestionBaseViewModel, new()
    {
      var question = new T
      {
        QuestionId = dbEntity.QuestionId,
        Condition = dbEntity.Condition,
        Options =
          new List<Option>
          {
            new Option(dbEntity.FirstAnswersCount, dbEntity.FirstOption),
            new Option(dbEntity.SecondAnswersCount, dbEntity.SecondOption)
          },
        BackgroundUrl = dbEntity.OriginalBackgroundUrl!=null ? MediaConverter.ToFullBackgroundurlUrl(dbEntity.OriginalBackgroundUrl, backgroundSizeType) : null,
        QuestionType = dbEntity.QuestionType,
        QuestionAddDate = dbEntity.AddedDate,
        Author = new AuthorViewModel
        {
          UserId = dbEntity.UserId,
          Login = dbEntity.Login,
          SexType = dbEntity.SexType,
          SmallAvatarUrl = dbEntity.OriginalAvatarUrl != null ?  MediaConverter.ToFullAvatarUrl(dbEntity.OriginalAvatarUrl, AvatarSizeType.Small) : null
        },
       
        LikesCount = dbEntity.LikesCount,
        DislikesCount = dbEntity.DislikesCount,
        YourFeedbackType = dbEntity.YourFeedbackType,
        YourAnswerType = dbEntity.YourAnswerType,
        IsInFavorites = dbEntity.IsInFavorites,
        IsSaved = dbEntity.IsSaved,
        CommentsCount = dbEntity.CommentsCount,
        Tags = dbTags.MapTagsDbToTagViewModel(),
        Photos = new List<List<PhotoViewModel>>(){ dbFirstPhotos.MapPhotosDbToViewModel(), dbSecondPhotos.MapPhotosDbToViewModel() }
      };
      return question;
    }

    public static List<TagViewModel> MapTagsDbToTagViewModel(this IEnumerable<TagDb> dbTags)
    {
      return dbTags.Select(tag => new TagViewModel
      {
        TagId = tag.TagId,
        Text = tag.Text
        // Position = tag.Position
      }).ToList();
    }

    public static List<PhotoViewModel> MapPhotosDbToViewModel(this IEnumerable<PhotoDb> photosDb)
    {
      return photosDb.Select(p => new PhotoViewModel
      {
        UserId = p.UserId,
        Age = p.BirthDate.Age(),
        Login = p.Login,
        SexType = p.SexType,
        SmallAvatarUrl = MediaConverter.ToFullAvatarUrl(p.OriginalAvatarUrl, CommonLibraries.AvatarSizeType.Small)
      }).ToList();
    }

    public static List<CommentViewModel> MapCommentsDbToViewModel(this IEnumerable<CommentDb> commentsDb)
    {
      return commentsDb.Select(c => new CommentViewModel
      {
        CommentId = c.CommentId,
        UserId = c.UserId,
        Login = c.Login,
        SmallAvatarUrl = MediaConverter.ToFullAvatarUrl(c.OriginalAvatarUrl, CommonLibraries.AvatarSizeType.Small),
        Text = c.Text,
        LikesCount = c.LikesCount,
        DislikesCount = c.DislikesCount,
        YourFeedbackType = c.YourFeedbackType,
        PreviousCommentId = c.PreviousCommentId.GetValueOrDefault(0),
        CommentedDate = c.CommentedDate
      }).ToList();
    }

    public static List<AnsweredListViewModel> MapAnsweredListDbToViewModel(
      this IEnumerable<AnsweredListDb> answeredList)
    {
      return answeredList.Select(v => new AnsweredListViewModel
      {
        UserId = v.UserId,
        Login = v.Login,
        SmallAvatarUrl = MediaConverter.ToFullAvatarUrl(v.OriginalAvatarUrl, CommonLibraries.AvatarSizeType.Small),
        Age = v.BirthDate.Age(),
        SexType = v.SexType,
        IsHeFollowed = v.IsHeFollowed,
        IsYouFollowed = v.IsYouFollowed
      }).ToList();
    }
  }
}
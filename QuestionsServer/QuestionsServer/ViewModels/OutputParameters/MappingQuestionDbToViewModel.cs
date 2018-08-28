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
    public static NewsAnsweredQuestionViewModel MapToNewsAnsweredQuestionsViewModel(this NewsAnsweredQuestionDto dto, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<NewsAnsweredQuestionViewModel>(dto.NewsAnsweredQuestionDb, mediaConverter, dbTags, dbFirstPhotos,
          dbSecondPhotos, backgroundSizeType);

      viewModel.AnsweredFollowingsCount = dto.NewsAnsweredQuestionDb.AnsweredFollowingsCount;
      viewModel.Priority = dto.Priority;
      viewModel.NewsType = CommonLibraries.NewsType.Answered;
      return viewModel;
    }

    public static NewsAskedQuestionViewModel MapToNewsAskedQuestionsViewModel(this NewsAskedQuestionDto dto, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<NewsAskedQuestionViewModel>(dto.NewsAskedQuestionDb, mediaConverter, dbTags, dbFirstPhotos,
          dbSecondPhotos, backgroundSizeType);

      viewModel.Priority = dto.Priority;
      viewModel.NewsType = CommonLibraries.NewsType.Asked;
      return viewModel;
    }

    public static NewsCommentedQuestionViewModel MapToNewsCommentedQuestionsViewModel(this NewsCommentedQuestionDto dto, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<NewsCommentedQuestionViewModel>(dto.NewsCommentedQuestionDb, mediaConverter, dbTags, dbFirstPhotos,
          dbSecondPhotos, backgroundSizeType);

      viewModel.CommentedUser = new NewsUserViewModel
      {
        UserId = dto.NewsCommentedQuestionDb.CommentedUserId,
        Login = dto.NewsCommentedQuestionDb.CommentedUserFirstName + " " + dto.NewsCommentedQuestionDb.CommentedUserLastName,
        SexType = dto.NewsCommentedQuestionDb.CommentedUserSexType
      };
      viewModel.UserCommentsCount = dto.NewsCommentedQuestionDb.CommentedUserCommentsCount;

      viewModel.Priority = dto.Priority;
      viewModel.NewsType = CommonLibraries.NewsType.Commented;

      return viewModel;
    }

    public static NewsFavoriteQuestionViewModel MapToNewsFavoriteQuestionsViewModel(this NewsFavoriteQuestionDto dto, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<NewsFavoriteQuestionViewModel>(dto.NewsFavoriteQuestionDb, mediaConverter, dbTags, dbFirstPhotos,
          dbSecondPhotos, backgroundSizeType);
      viewModel.FavoriteAddedUser = new NewsUserViewModel
      {
        UserId = dto.NewsFavoriteQuestionDb.FavoriteAddedUserId,
        SexType = dto.NewsFavoriteQuestionDb.FavoriteAddedUserSexType,
        Login = dto.NewsFavoriteQuestionDb.FavoriteAddedUserFirstName + " " + dto.NewsFavoriteQuestionDb.FavoriteAddedUserLastName
      };

      viewModel.Priority = dto.Priority;
      viewModel.NewsType = CommonLibraries.NewsType.Favorite;
      return viewModel;
    }

    public static NewsRecommendedQuestionViewModel MapNewsRecommendedQuestionsViewModel(
      this NewsRecommendedQuestionDto dto, MediaConverter mediaConverter, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
      IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<NewsRecommendedQuestionViewModel>(dto, mediaConverter, dbTags, dbFirstPhotos,
          dbSecondPhotos, backgroundSizeType);

      viewModel.RecommendedUsers = new List<NewsUserViewModel>(dto.RecommendedUsers.Count);

      foreach (var user in dto.RecommendedUsers)
      {
        viewModel.RecommendedUsers.Add(new NewsUserViewModel
        {
          UserId = user.UserId,
          Login = user.FirstName + " " + user.LastName,
          SexType = user.SexType
        });
      }



      viewModel.Priority = dto.Priority;
      viewModel.NewsType = NewsType.Recommended;

      return viewModel;
    }

    public static UserAnsweredQuestionsViewModel MapToUserAnsweredQuestionsViewModel(
      this UserAnsweredQuestionDb dbEntity, MediaConverter mediaConverter,  IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
      IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<UserAnsweredQuestionsViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static UserAskedQuestionsViewModel MapToUserAskedQuestionsViewModel(this UserAskedQuestionDb dbEntity, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<UserAskedQuestionsViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static UserCommentedQuestionsViewModel MapToUserCommentedQuestionsViewModel(
      this UserCommentedQuestionDto dbEntity, MediaConverter mediaConverter, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
      IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<UserCommentedQuestionsViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);

      viewModel.Comments = dbEntity.Comments;
      return viewModel;
    }

    public static UserFavoriteQuestionsViewModel MapToUserFavoriteQuestionsViewModel(
      this UserFavoriteQuestionDb dbEntity, MediaConverter mediaConverter, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
      IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<UserFavoriteQuestionsViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static TopQuestionsViewModel MapToTopQuestionsViewModel(this TopQuestionDb dbEntity, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<TopQuestionsViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static TopQuestionsViewModel MapToTopQuestionsViewModelMod( this QuestionDto dbEntity, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = new TopQuestionsViewModel
      {
        QuestionId = dbEntity.QuestionId,
        Condition = dbEntity.Condition,
        Options = dbEntity.Options.Select(x => new Option { Text = x.Text, Voters = x.Voters }).ToList(),
        Author = new AuthorViewModel
        {
          UserId = dbEntity.Author.UserId,
          Login = dbEntity.Author.FirstName + " " + dbEntity.Author.LastName,
          SexType = dbEntity.Author.SexType,
          SmallAvatarUrl = mediaConverter.ToFullAvatarUrl(dbEntity.Author.OriginalAvatarUrl, AvatarSizeType.Small)

        },
        BackgroundUrl = mediaConverter.ToFullBackgroundurlUrl(dbEntity.OriginalBackgroundUrl, backgroundSizeType),
        QuestionType = dbEntity.QuestionType,
        QuestionAddDate = dbEntity.AddedDate,
        LikesCount = dbEntity.LikesCount,
        DislikesCount = dbEntity.DislikesCount,
        YourAnswerType = dbEntity.YourAnswerType,
        YourFeedbackType = dbEntity.YourFeedbackType,


        IsFavorite = dbEntity.IsFavorite,
        IsSaved = dbEntity.IsSaved,
        CommentsCount = dbEntity.CommentsCount,
        Photos = new List<List<PhotoViewModel>>{MapPhotosDbToViewModel(dbFirstPhotos, mediaConverter),
   MapPhotosDbToViewModel(dbSecondPhotos, mediaConverter)},
        Tags = MapTagsDbToTagViewModel(dbTags)


        // FirstOption = dbEntity.Options.FirstOrDefault(x=>x.)
      };
      return viewModel;
    }


    public static AskedQuestionsViewModel MapToAskedQuestionsViewModel(this AskedQuestionDb dbEntity, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<AskedQuestionsViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static SelectedQuestionsViewModel MapToChosenQuestionsViewModel(this SelectedQuestionDb dbEntity, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<SelectedQuestionsViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }


    public static RecommendedQuestionViewModel MapToRecommendedQuestionsViewModel(this RecommendedQuestionDb dbEntity, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<RecommendedQuestionViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      //viewModel.ToUserId = dbEntity.ToUserId;
      //viewModel.ToUserLogin = dbEntity.ToUserLogin;
      //viewModel.RecommendDate = dbEntity.RecommendDate;

      return viewModel;
    }

    public static RecommendedQuestionViewModel MapToRecommendedQuestionsViewModel(this RecommendedQuestionDto dbEntity, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel =
        QuestionDbToViewModel<RecommendedQuestionViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);

      viewModel.Users = dbEntity.RecommendedToUsers;
      return viewModel;
    }

    public static LikedQuestionsViewModel MapToLikedQuestionsViewModel(this LikedQuestionDb dbEntity, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<LikedQuestionsViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static SavedQuestionsViewModel MapToSavedQuestionsViewModel(this SavedQuestionDb dbEntity, MediaConverter mediaConverter,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<SavedQuestionsViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      return viewModel;
    }

    public static GetQuestionViewModel MapToGetQuestionsViewModel(this QuestionDb dbEntity, MediaConverter mediaConverter, IEnumerable<TagDb> dbTags,
      IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> comments, BackgroundSizeType backgroundSizeType)
    {
      var viewModel = QuestionDbToViewModel<GetQuestionViewModel>(dbEntity, mediaConverter, dbTags, dbFirstPhotos, dbSecondPhotos, backgroundSizeType);
      viewModel.Comments = comments.MapCommentsDbToViewModel(mediaConverter);
      return viewModel;
    }

    public static T QuestionDbToViewModel<T>(this QuestionBaseDb dbEntity, MediaConverter mediaConverter, IEnumerable<TagDb> dbTags,
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
        BackgroundUrl = dbEntity.OriginalBackgroundUrl != null ? mediaConverter.ToFullBackgroundurlUrl(dbEntity.OriginalBackgroundUrl, backgroundSizeType) : null,
        QuestionType = dbEntity.QuestionType,
        QuestionAddDate = dbEntity.AddedDate,
        Author = new AuthorViewModel
        {
          UserId = dbEntity.UserId,
          Login = dbEntity.FirstName + " " + dbEntity.LastName,
          SexType = dbEntity.SexType,
          SmallAvatarUrl = dbEntity.OriginalAvatarUrl != null ? mediaConverter.ToFullAvatarUrl(dbEntity.OriginalAvatarUrl, AvatarSizeType.Small) : null
        },

        LikesCount = dbEntity.LikesCount,
        DislikesCount = dbEntity.DislikesCount,
        YourFeedbackType = dbEntity.YourFeedbackType,
        YourAnswerType = dbEntity.YourAnswerType,
        IsFavorite = dbEntity.IsFavorite,
        IsSaved = dbEntity.IsSaved,
        CommentsCount = dbEntity.CommentsCount,
        Tags = dbTags.MapTagsDbToTagViewModel(),
        Photos = new List<List<PhotoViewModel>>() { dbFirstPhotos.MapPhotosDbToViewModel(mediaConverter), dbSecondPhotos.MapPhotosDbToViewModel(mediaConverter) }
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

    public static List<PhotoViewModel> MapPhotosDbToViewModel(this IEnumerable<PhotoDb> photosDb, MediaConverter mediaConverter)
    {
      return photosDb.Select(p => new PhotoViewModel
      {
        UserId = p.UserId,
        Age = p.BirthDate.Age(),
        Login = p.FirstName + " " + p.LastName,
        SexType = p.SexType,
        SmallAvatarUrl =  mediaConverter.ToFullAvatarUrl(p.OriginalAvatarUrl, AvatarSizeType.Small)
      }).ToList();
    }

    public static List<CommentViewModel> MapCommentsDbToViewModel(this IEnumerable<CommentDb> commentsDb, MediaConverter mediaConverter)
    {
      return commentsDb.Select(c => new CommentViewModel
      {
        CommentId = c.CommentId,
        UserId = c.UserId,
        Login = c.FirstName + " " + c.LastName,
        SmallAvatarUrl = mediaConverter.ToFullAvatarUrl(c.OriginalAvatarUrl, AvatarSizeType.Small),
        Text = c.Text,
        LikesCount = c.LikesCount,
        DislikesCount = c.DislikesCount,
        YourFeedbackType = c.YourFeedbackType,
        PreviousCommentId = c.PreviousCommentId.GetValueOrDefault(0),
        CommentedDate = c.CommentedDate
      }).ToList();
    }

    public static List<AnsweredListViewModel> MapAnsweredListDbToViewModel(
      this IEnumerable<AnsweredListDb> answeredList, MediaConverter mediaConverter)
    {
      return answeredList.Select(v => new AnsweredListViewModel
      {
        UserId = v.UserId,
        Login = v.FirstName + " " + v.LastName,
        SmallAvatarUrl = mediaConverter.ToFullAvatarUrl(v.OriginalAvatarUrl, AvatarSizeType.Small),
        Age = v.BirthDate.Age(),
        SexType = v.SexType,
        IsHeFollowed = v.IsHeFollowed,
        IsYouFollowed = v.IsYouFollowed
      }).ToList();
    }
  }
}
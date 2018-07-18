using System.Collections.Generic;
using System.Linq;
using CommonLibraries.Extensions;
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
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel =
        QuestionDbToViewModel<NewsAnsweredQuestionViewModel>(dto.NewsAnsweredQuestionDb, dbTags, dbFirstPhotos,
          dbSecondPhotos);

      //viewModel.AnsweredFollowToAmount = dto.NewsAnsweredQuestionDb.AnsweredFollowTo;
      viewModel.Priority = dto.Priority;
      viewModel.NewsType =  CommonLibraries.NewsType.Answered;
      return viewModel;
    }

    public static NewsAskedQuestionViewModel MapToNewsAskedQuestionsViewModel(this NewsAskedQuestionDto dto,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel =
        QuestionDbToViewModel<NewsAskedQuestionViewModel>(dto.NewsAskedQuestionDb, dbTags, dbFirstPhotos,
          dbSecondPhotos);

      viewModel.Priority = dto.Priority;
      viewModel.NewsType = CommonLibraries.NewsType.Asked;
      return viewModel;
    }

    public static NewsCommentedQuestionViewModel MapToNewsCommentedQuestionsViewModel(this NewsCommentedQuestionDto dto,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel =
        QuestionDbToViewModel<NewsCommentedQuestionViewModel>(dto.NewsCommentedQuestionDb, dbTags, dbFirstPhotos,
          dbSecondPhotos);

      viewModel.CommentedUser = new NewsUserViewModel
      {
        UserId = dto.NewsCommentedQuestionDb.CommentUserId,
        Login = dto.NewsCommentedQuestionDb.CommentUserLogin,
        SexType = dto.NewsCommentedQuestionDb.CommentUserSexType
      };
      viewModel.UserCommentsAmount = dto.NewsCommentedQuestionDb.CommentsAmount;

      viewModel.Priority = dto.Priority;
      viewModel.NewsType = CommonLibraries.NewsType.Commented;

      return viewModel;
    }

    public static NewsFavoriteQuestionViewModel MapToNewsFavoriteQuestionsViewModel(this NewsFavoriteQuestionDto dto,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel =
        QuestionDbToViewModel<NewsFavoriteQuestionViewModel>(dto.NewsFavoriteQuestionDb, dbTags, dbFirstPhotos,
          dbSecondPhotos);
      viewModel.FavoriteAddedUser = new NewsUserViewModel
      {
        UserId = dto.NewsFavoriteQuestionDb.FavoriteAddedUserId,
        SexType = dto.NewsFavoriteQuestionDb.FavoriteAddedSexType,
        Login = dto.NewsFavoriteQuestionDb.FavoriteAddedUserLogin
      };

      viewModel.Priority = dto.Priority;
      viewModel.NewsType = CommonLibraries.NewsType.Favorite;
      return viewModel;
    }

    public static NewsRecommendedQuestionViewModel MapNewsRecommendedQuestionsViewModel(
      this NewsRecommendedQuestionDto dto, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
      IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel =
        QuestionDbToViewModel<NewsRecommendedQuestionViewModel>(dto, dbTags, dbFirstPhotos,
          dbSecondPhotos);

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
      IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel =
        QuestionDbToViewModel<UserAnsweredQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
      return viewModel;
    }

    public static UserAskedQuestionsViewModel MapToUserAskedQuestionsViewModel(this UserAskedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel =
        QuestionDbToViewModel<UserAskedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
      return viewModel;
    }

    public static UserCommentedQuestionsViewModel MapToUserCommentedQuestionsViewModel(
      this UserCommentedQuestionDto dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
      IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel =
        QuestionDbToViewModel<UserCommentedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);

      viewModel.Comments = dbEntity.Comments;
      return viewModel;
    }

    public static UserFavoriteQuestionsViewModel MapToUserFavoriteQuestionsViewModel(
      this UserFavoriteQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
      IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel =
        QuestionDbToViewModel<UserFavoriteQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
      return viewModel;
    }

    public static TopQuestionsViewModel MapToTopQuestionsViewModel(this TopQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel = QuestionDbToViewModel<TopQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
      return viewModel;
    }

    public static AskedQuestionsViewModel MapToAskedQuestionsViewModel(this AskedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel = QuestionDbToViewModel<AskedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
      return viewModel;
    }

    public static SelectedQuestionsViewModel MapToChosenQuestionsViewModel(this SelectedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel = QuestionDbToViewModel<SelectedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
      return viewModel;
    }


    public static RecommendedQuestionViewModel MapToRecommendedQuestionsViewModel(this RecommendedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel =
        QuestionDbToViewModel<RecommendedQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
      //viewModel.ToUserId = dbEntity.ToUserId;
      //viewModel.ToUserLogin = dbEntity.ToUserLogin;
      //viewModel.RecommendDate = dbEntity.RecommendDate;

      return viewModel;
    }

    public static RecommendedQuestionViewModel MapToRecommendedQuestionsViewModel(this RecommendedQuestionDto dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel =
        QuestionDbToViewModel<RecommendedQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);

      viewModel.Users = dbEntity.RecommendedToUsers;
      return viewModel;
    }

    public static LikedQuestionsViewModel MapToLikedQuestionsViewModel(this LikedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel = QuestionDbToViewModel<LikedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
      return viewModel;
    }

    public static SavedQuestionsViewModel MapToSavedQuestionsViewModel(this SavedQuestionDb dbEntity,
      IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
    {
      var viewModel = QuestionDbToViewModel<SavedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
      return viewModel;
    }

    public static GetQuestionViewModel MapToGetQuestionsViewModel(this QuestionDb dbEntity, IEnumerable<TagDb> dbTags,
      IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> comments)
    {
      var viewModel = QuestionDbToViewModel<GetQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
      viewModel.Comments = comments.MapCommentsDbToViewModel();
      return viewModel;
    }

    public static T QuestionDbToViewModel<T>(this QuestionBaseDb dbEntity, IEnumerable<TagDb> dbTags,
      IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos) where T : QuestionBaseViewModel, new()
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
        BackgroundImageLink = dbEntity.BackgroundImageLink,
        QuestionType = dbEntity.QuestionType,
        QuestionAddDate = dbEntity.QuestionAddDate,
        Author = new AuthorViewModel
        {
          UserId = dbEntity.UserId,
          Login = dbEntity.Login,
          SexType = dbEntity.SexType,
          SmallAvatarLink = dbEntity.SmallAvatarLink,
        },
       
        LikesAmount = dbEntity.LikesCount,
        DislikesAmount = dbEntity.DislikesCount,
        YourFeedbackType = dbEntity.YourFeedback,
        YourAnswerType = dbEntity.YourAnswer,
        IsInFavorites = dbEntity.InFavorites,
        IsSaved = dbEntity.IsSaved,
        CommentsAmount = dbEntity.CommentsCount,
        Tags = dbTags.MapTagsDbToTagViewModel(),
        FirstPhotos = dbFirstPhotos.MapPhotosDbToViewModel(),
        SecondPhotos = dbSecondPhotos.MapPhotosDbToViewModel()
      };
      return question;
    }

    public static List<TagViewModel> MapTagsDbToTagViewModel(this IEnumerable<TagDb> dbTags)
    {
      return dbTags.Select(tag => new TagViewModel
      {
        TagId = tag.TagId,
        TagText = tag.TagText
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
        SmallAvatarLink = p.SmallAvatarLink
      }).ToList();
    }

    public static List<CommentViewModel> MapCommentsDbToViewModel(this IEnumerable<CommentDb> commentsDb)
    {
      return commentsDb.Select(c => new CommentViewModel
      {
        CommentId = c.CommentId,
        UserId = c.UserId,
        Login = c.Login,
        SmallAvatarLink = c.SmallAvatarLink,
        Text = c.Comment,
        LikesAmount = c.Likes,
        DislikesAmount = c.Dislikes,
        YourFeedbackType = c.YourFeedback,
        PreviousCommentId = c.PreviousCommentId.GetValueOrDefault(0),
        AddDate = c.CommentAddDate
      }).ToList();
    }

    public static List<AnsweredListViewModel> MapAnsweredListDbToViewModel(
      this IEnumerable<AnsweredListDb> answeredList)
    {
      return answeredList.Select(v => new AnsweredListViewModel
      {
        UserId = v.UserId,
        Login = v.Login,
        SmallAvatarLink = v.SmallAvatarLink,
        Age = v.BirthDate.Age(),
        SexType = v.SexType,
        IsHeFollowed = v.HeFollowed,
        IsYouFollowed = v.YouFollowed
      }).ToList();
    }
  }
}
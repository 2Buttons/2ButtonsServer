using AccountData.Main.Queries;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders;

namespace AccountServer.ViewModels
{
  public static class MappingUserDbToViewModel
  {
    public static UserInfoViewModel MapToUserInfoViewModel(
      this UserInfoDb dbEntity)
    {
      var viewModel = new UserInfoViewModel
      {
        UserId = dbEntity.UserId,
        Login = dbEntity.Login,
        Age = dbEntity.BirthDate.Age(),
        SexType =  dbEntity.SexType,
        City = dbEntity.City,
        Description = dbEntity.Description,
        LargeAvatarUrl = MediaConverter.ToFullAvatarUrl(dbEntity.OriginalAvatarUrl, AvatarSizeType.Large),
        SmallAvatarUrl = MediaConverter.ToFullAvatarUrl(dbEntity.OriginalAvatarUrl, AvatarSizeType.Small),
        IsYouFollowed = dbEntity.YouFollowed,
        IsHeFollowed = dbEntity.HeFollowed,

        AskedQuestionsCount = dbEntity.AskedQuestions,
        AnswersCount = dbEntity.Answers,
        FollowersCount = dbEntity.Followers,
        FollowedCount = dbEntity.Followed,
        FavoritesCount = dbEntity.Favorites,
        CommentsCount = dbEntity.Comments,
      };
      return viewModel;
    }

    public static UserStatisticsViewModel MapToUserStatisticsViewModel(
      this UserStatisticsDb dbEntity)
    {
      var viewModel = new UserStatisticsViewModel
      {
        PublicAskedQuestions = dbEntity.PublicAskedQuestions,
        AnsweredQuestions = dbEntity.AnsweredQuestions,
        PublicAnsweredQuestions = dbEntity.PublicAnsweredQuestions,
        PublicFavoriteQuestions = dbEntity.PublicFavoriteQuestions,
        CommentsWritten = dbEntity.CommentsWritten,

        UserQuestionsShows = dbEntity.UserQuestionsShows,
        UserQuestionsAnswers = dbEntity.UserQuestionsAnswers,

        QuestionsCommentsGot = dbEntity.QuestionsCommentsGot,
        QuestionsLikesGot = dbEntity.QuestionsLikesGot,
        QuestionsDislikesGot = dbEntity.QuestionsDislikesGot,
        CommentsLikesGot = dbEntity.CommentsLikesGot,
        CommentsDislikesGot = dbEntity.CommentsDislikesGot,
        QuestionsLikesMade = dbEntity.QuestionsLikesMade,
        QuestionsDislikesMade = dbEntity.QuestionsDislikesMade,
        CommentsLikesMade = dbEntity.CommentsLikesMade,
        CommentsDislikesMade = dbEntity.CommentsDislikesMade
      };
      return viewModel;
    }
  }
}
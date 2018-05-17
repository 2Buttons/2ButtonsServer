using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
using CommonLibraries.Extensions;
using TwoButtonsDatabase.Entities.Account;

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
        SexType = (SexType) dbEntity.Sex,
        Description = dbEntity.Description,
        FullAvatarLink = dbEntity.FullAvatarLink,
        SmallAvatarLink = dbEntity.SmallAvatarLink,
        IsYouFollowed = dbEntity.YouFollowed,
        IsHeFollowed = dbEntity.HeFollowed,

        AskedQuestionsAmount = dbEntity.AskedQuestions,
        AnswersAmount = dbEntity.Answers,
        FollowersAmount = dbEntity.Followers,
        FollowedAmount = dbEntity.Followed,
        FavoritesAmount = dbEntity.Favorites,
        CommentsAmount = dbEntity.Comments,
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

    //public static List<UserContactsViewModel> MapToUserContactsViewModel(
    //  this IEnumerable<UserContactsDb> userContactsDb)
    //{
    //  return userContactsDb.Select(contact => new UserContactsViewModel
    //    {
    //      ContactsAccount = contact.Account,
    //      NetworkType = (SocialNetType) contact.NetworkId
    //    })
    //    .ToList();
    //}
  }
}
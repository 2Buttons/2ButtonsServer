using TwoButtonsDatabase.Entities;
using TwoButtonsServer.ViewModels.User;

namespace TwoButtonsServer.ViewModels
{
    public static class MappintUserDbToViewModel
    {
        public static UserInfoViewModel MapToUserInfoViewModel(
            this UserInfoDb dbEntity, UserStatisticsDb statisticsDb)
        {
            var viewModel = new UserInfoViewModel
            {
                UserId = dbEntity.UserId,
                Login = dbEntity.Login,
                Age = dbEntity.Age,
                Sex = dbEntity.Sex,
                Description = dbEntity.Description,
                FullAvatarLink = dbEntity.FullAvatarLink,
                YouFollowed = dbEntity.YouFollowed,
                HeFollowed = dbEntity.HeFollowed,
                AskedQuestions = dbEntity.AskedQuestions,
                Answers = dbEntity.Answers,
                Followers = dbEntity.Followers,
                Followed = dbEntity.Followed,
                Favorites = dbEntity.Favorites,
                Comments = dbEntity.Comments,

                UserStatistics = statisticsDb.MapToUserStatisticsViewModel()
            };
            return viewModel;
        }

        public static UserStatisticsViewModel MapToUserStatisticsViewModel(
            this UserStatisticsDb dbEntity)
        {
            var viewModel = new UserStatisticsViewModel
            {
                PublicAskedQuestions = dbEntity.PublicAskedQuestions,
                AskedQuestions = dbEntity.AskedQuestions,
                AnsweredQuestions = dbEntity.AnsweredQuestions,
                SeenQuestions = dbEntity.SeenQuestions,
                PublicFavoriteQuestions = dbEntity.PublicFavoriteQuestions,
                FavoriteQuestions = dbEntity.FavoriteQuestions,
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
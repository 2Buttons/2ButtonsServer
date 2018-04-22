using System.Collections.Generic;
using System.Linq;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.NewsQuestions;
using TwoButtonsDatabase.Entities.UserQuestions;
using TwoButtonsServer.ViewModels.News;
using TwoButtonsServer.ViewModels.UserQuestions;



namespace TwoButtonsServer.ViewModels
{
    public static class MappingQuestionDbToViewModel
    {

        public static NewsAnsweredQuestionViewModel MapToNewsAnsweredQuestionsViewModel(
            this NewsAnsweredQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<NewsAnsweredQuestionViewModel>(dbEntity, dbTags);
            viewModel.AnsweredFollowTo = dbEntity.AnsweredFollowTo;
            viewModel.AnswerDate = dbEntity.AnswerDate;
            return viewModel;
        }

        public static NewsAskedQuestionViewModel MapToNewsAskedQuestionsViewModel(
            this NewsAskedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            return QuestionDbToViewModel<NewsAskedQuestionViewModel>(dbEntity, dbTags);
        }

        public static NewsCommentedQuestionViewModel MapToNewsCommentedQuestionsViewModel(
            this NewsCommentedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<NewsCommentedQuestionViewModel>(dbEntity, dbTags);

            viewModel.CommentUserId = dbEntity.CommentUserId;
            viewModel.CommentUserLogin = dbEntity.CommentUserLogin;
            viewModel.CommentsAmount = dbEntity.CommentsAmount;
            viewModel.CommentAddDate = dbEntity.CommentAddDate;
            return viewModel;
        }

        public static NewsFavouriteQuestionViewModel MapToNewsFavouriteQuestionsViewModel(
            this NewsFavouriteQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<NewsFavouriteQuestionViewModel>(dbEntity, dbTags);
            viewModel.FavoriteAddedUserId = dbEntity.FavoriteAddedUserId;
            viewModel.FavoriteAddedUserLogin = dbEntity.FavoriteAddedUserLogin;
            viewModel.FavoriteAddDate = dbEntity.FavoriteAddDate;
            return viewModel;
        }


        public static NewsRecommendedQuestionViewModel MapNewsRecommendedQuestionsViewModel(
            this NewsRecommendedQuestionDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<NewsRecommendedQuestionViewModel>(dbEntity, dbTags);
            viewModel.RecommendedUserId = dbEntity.RecommendedUserId;
            viewModel.RecommendedUserLogin = dbEntity.RecommendedUserLogin;
            viewModel.RecommendedDate = dbEntity.RecommendedDate;
            return viewModel;
        }


        public static UserAnsweredQuestionsViewModel MapToUserAnsweredQuestionsViewModel(
            this UserAnsweredQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<UserAnsweredQuestionsViewModel>(dbEntity, dbTags);
            return viewModel;
        }

        public static UserAskedQuestionsViewModel MapToUserAskedQuestionsViewModel(
            this UserAskedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<UserAskedQuestionsViewModel>(dbEntity, dbTags);
            return viewModel;
        }

        public static UserCommentedQuestionsViewModel MapToUserCommentedQuestionsViewModel(
            this UserCommentedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<UserCommentedQuestionsViewModel>(dbEntity, dbTags);

            viewModel.CommentId = dbEntity.CommentId;
            viewModel.Comment = dbEntity.Comment;
            viewModel.CommentLikes = dbEntity.CommentLikes;
            viewModel.CommentDislikes = dbEntity.CommentDislikes;
            viewModel.YourCommentFeedback = dbEntity.YourFeedback;
            viewModel.PreviousCommentId = dbEntity.PreviousCommentId;
            viewModel.CommentAddDate = dbEntity.CommentAddDate;
            return viewModel;
        }

        public static UserFavouriteQuestionsViewModel MapToUserFavouriteQuestionsViewModel(
            this UserFavouriteQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<UserFavouriteQuestionsViewModel>(dbEntity, dbTags);
            return viewModel;
        }


        public static TopQuestionsViewModel MapToTopQuestionsViewModel(
            this TopQuestionDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<TopQuestionsViewModel>(dbEntity, dbTags);
            return viewModel;
        }

        private static T QuestionDbToViewModel<T>(QuestionBaseDb dbEntity, IEnumerable<TagDb> dbTags) where T : QuestionBaseViewModel, new()
        {
            var question = new T
            {
                QuestionId = dbEntity.QuestionId,
                Condition = dbEntity.Condition,
                FirstOption = dbEntity.FirstOption,
                SecondOption = dbEntity.SecondOption,
                BackgroundImageLink = dbEntity.BackgroundImageLink,
                QuestionType = dbEntity.QuestionType,
                QuestionAddDate = dbEntity.QuestionAddDate,
                UserId = dbEntity.UserId,
                Login = dbEntity.Login,
                SmallAvatarLink = dbEntity.SmallAvatarLink,
                Answers = dbEntity.Answers,
                Likes = dbEntity.Likes,
                Dislikes = dbEntity.Dislikes,
                YourFeedback = dbEntity.YourFeedback,
                YourAnswer = dbEntity.YourAnswer,
                InFavorites = dbEntity.InFavorites,
                Comments = dbEntity.Comments,

                Tags = TagDbToTagViewModel(dbTags)
            };
            return question;
        }

        private static List<TagViewModel> TagDbToTagViewModel(IEnumerable<TagDb> dbTags)
        {
            return dbTags.Select(tag => new TagViewModel
            {
                TagId = tag.TagId,
                TagText = tag.TagText,
                Position = tag.Position
            })
                .ToList();
        }
    }
}
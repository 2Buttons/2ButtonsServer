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
            this NewsAnsweredQuestionsDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            var viewModel =
                QuestionDbToViewModel<NewsAnsweredQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
            viewModel.AnsweredFollowTo = dbEntity.AnsweredFollowTo;
            viewModel.AnswerDate = dbEntity.AnswerDate;
            return viewModel;
        }

        public static NewsAskedQuestionViewModel MapToNewsAskedQuestionsViewModel(
            this NewsAskedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            return QuestionDbToViewModel<NewsAskedQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
        }

        public static NewsCommentedQuestionViewModel MapToNewsCommentedQuestionsViewModel(
            this NewsCommentedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            var viewModel =
                QuestionDbToViewModel<NewsCommentedQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);

            viewModel.CommentUserId = dbEntity.CommentUserId;
            viewModel.CommentUserLogin = dbEntity.CommentUserLogin;
            viewModel.CommentsAmount = dbEntity.CommentsAmount;
            viewModel.CommentAddDate = dbEntity.CommentAddDate;
            return viewModel;
        }

        public static NewsFavouriteQuestionViewModel MapToNewsFavouriteQuestionsViewModel(
            this NewsFavouriteQuestionsDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            var viewModel =
                QuestionDbToViewModel<NewsFavouriteQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
            viewModel.FavoriteAddedUserId = dbEntity.FavoriteAddedUserId;
            viewModel.FavoriteAddedUserLogin = dbEntity.FavoriteAddedUserLogin;
            viewModel.FavoriteAddDate = dbEntity.FavoriteAddDate;
            return viewModel;
        }


        public static NewsRecommendedQuestionViewModel MapNewsRecommendedQuestionsViewModel(
            this NewsRecommendedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            var viewModel =
                QuestionDbToViewModel<NewsRecommendedQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos,
                    dbSecondPhotos);
            viewModel.RecommendedUserId = dbEntity.RecommendedUserId;
            viewModel.RecommendedUserLogin = dbEntity.RecommendedUserLogin;
            viewModel.RecommendedDate = dbEntity.RecommendedDate;
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

        public static UserAskedQuestionsViewModel MapToUserAskedQuestionsViewModel(
            this UserAskedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            var viewModel =
                QuestionDbToViewModel<UserAskedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
            return viewModel;
        }

        public static UserCommentedQuestionsViewModel MapToUserCommentedQuestionsViewModel(
            this UserCommentedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            var viewModel =
                QuestionDbToViewModel<UserCommentedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);

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
            this UserFavouriteQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            var viewModel =
                QuestionDbToViewModel<UserFavouriteQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
            return viewModel;
        }


        public static TopQuestionsViewModel MapToTopQuestionsViewModel(
            this TopQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            var viewModel =
                QuestionDbToViewModel<TopQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
            return viewModel;
        }


        public static AskedQuestionsViewModel MapToAskedQuestionsViewModel(
            this AskedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            var viewModel =
                QuestionDbToViewModel<AskedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
            return viewModel;
        }

        public static LikedQuestionsViewModel MapToLikedQuestionsViewModel(
            this LikedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            var viewModel =
                QuestionDbToViewModel<LikedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
            return viewModel;
        }

        public static SavedQuestionsViewModel MapToSavedQuestionsViewModel(
            this SavedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos)
        {
            var viewModel =
                QuestionDbToViewModel<SavedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos);
            return viewModel;
        }


        private static T QuestionDbToViewModel<T>(QuestionBaseDb dbEntity, IEnumerable<TagDb> dbTags,
            IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos)
            where T : QuestionBaseViewModel, new()
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
                Shows = dbEntity.Shows,
                Likes = dbEntity.Likes,
                Dislikes = dbEntity.Dislikes,
                YourFeedback = dbEntity.YourFeedback,
                YourAnswer = dbEntity.YourAnswer,
                InFavorites = dbEntity.InFavorites,
                Comments = dbEntity.Comments,

                FirstAnswers = dbEntity.FirstAnswers,
                SecondAnswers = dbEntity.SecondAnswers,

                Tags = dbTags.TagDbToTagViewModel(),
                FirstPhotos = dbFirstPhotos.PhotoDbToViewModel(),
                SecondPhotos = dbSecondPhotos.PhotoDbToViewModel()
            };
            return question;
        }

        public static List<TagViewModel> TagDbToTagViewModel(this IEnumerable<TagDb> dbTags)
        {
            return dbTags.Select(tag => new TagViewModel
                {
                    TagId = tag.TagId,
                    TagText = tag.TagText,
                    Position = tag.Position
                })
                .ToList();
        }

        public static List<PhotoViewModel> PhotoDbToViewModel(this IEnumerable<PhotoDb> photosDb)
        {
            return photosDb.Select(p => new PhotoViewModel
                {
                    UserId = p.UserId,
                    Age = p.Age,
                    Login = p.Login,
                    Sex = p.Sex,
                    SmallAvatarLink = p.SmallAvatarLink
                })
                .ToList();
        }
    }
}
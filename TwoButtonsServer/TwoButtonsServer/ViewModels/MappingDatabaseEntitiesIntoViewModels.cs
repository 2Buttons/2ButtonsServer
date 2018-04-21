using System.Collections.Generic;
using System.Linq;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.UserQuestions;
using TwoButtonsServer.ViewModels.UserQuestions;
using TagViewModel = TwoButtonsServer.ViewModels.News.TagViewModel;
using UserAnsweredQuestionsDb = TwoButtonsDatabase.Entities.News.UserAnsweredQuestionsDb;
using UserAskedQuestionsDb = TwoButtonsDatabase.Entities.News.UserAskedQuestionsDb;
using UserCommentedQuestionsDb = TwoButtonsDatabase.Entities.News.UserCommentedQuestionsDb;
using UserFavouriteQuestionsDb = TwoButtonsDatabase.Entities.News.UserFavouriteQuestionsDb;

namespace TwoButtonsServer.ViewModels
{
    public static class MappingDatabaseEntitiesIntoViewModels
    {
        public static UserAnsweredQuestionsViewModel MapToUserAnsweredQuestionsViewModel(
            this UserAnsweredQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<UserAnsweredQuestionsViewModel>(dbEntity);
            viewModel.Tags = TagDbToTagViewModel(dbTags);
            return viewModel;
        }

        public static UserAskedQuestionsViewModel MapToUserAskedQuestionsViewModel(
            this UserAskedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<UserAskedQuestionsViewModel>(dbEntity);
            viewModel.Tags = TagDbToTagViewModel(dbTags);
            return viewModel;
        }

        public static UserCommentedQuestionsViewModel MapToUserCommentedQuestionsViewModel(
            this UserCommentedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<UserCommentedQuestionsViewModel>(dbEntity);

            viewModel.CommentId = dbEntity.CommentId;
            viewModel.Comment = dbEntity.Comment;
            viewModel.CommentLikes = dbEntity.CommentLikes;
            viewModel.CommentDislikes = dbEntity.CommentDislikes;
            viewModel.YourCommentFeedback = dbEntity.YourFeedback;
            viewModel.PreviousCommentId = dbEntity.PreviousCommentId;
            viewModel.CommentAddDate = dbEntity.CommentAddDate;
            viewModel.Tags = TagDbToTagViewModel(dbTags);
            return viewModel;
        }

        public static UserFavouriteQuestionsViewModel MapToUserFavouriteQuestionsViewModel(
            this UserFavouriteQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<UserFavouriteQuestionsViewModel>(dbEntity);
            viewModel.Tags = TagDbToTagViewModel(dbTags);
            return viewModel;
        }


        public static TopQuestionsViewModel MapToTopQuestionsViewModel(
            this TopQuestionDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var viewModel = QuestionDbToViewModel<TopQuestionsViewModel>(dbEntity);
            viewModel.Tags = TagDbToTagViewModel(dbTags);
            return viewModel;
        }

        private static T QuestionDbToViewModel<T>(QuestionBaseDb dbEntity) where T:QuestionBaseViewModel, new()
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
                Comments = dbEntity.Comments
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
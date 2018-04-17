using System.Collections.Generic;
using System.Linq;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsServer.ViewModels
{
    public static class MappingDatabaseEntitiesIntoViewModels
    {
        public static UserAnsweredQuestionsViewModel MapToUserAnsweredQuestionsViewModel(
            this UserAnsweredQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var tags = dbTags.Select(tag => new TagViewModel
            {
                TagId = tag.TagId,
                TagText = tag.TagText,
                Position = tag.Position
            })
                .ToList();

            var viewModel = new UserAnsweredQuestionsViewModel
            {
                QuestionId = dbEntity.QuestionId,
                Condition = dbEntity.Condition,
                FirstOption = dbEntity.FirstOption,
                SecondOption = dbEntity.SecondOption,
                QuestionType = dbEntity.QuestionType,
                QuestionAddDate = dbEntity.QuestionAddDate,
                UserId = dbEntity.UserId,
                Login = dbEntity.Login,
                AvatarLink = dbEntity.AvatarLink,
                Answers = dbEntity.Answers,
                Likes = dbEntity.Likes,
                Dislikes = dbEntity.Dislikes,
                YourFeedback = dbEntity.YourFeedback,
                YourAnswer = dbEntity.YourAnswer,
                InFavorites = dbEntity.InFavorites,
                Comments = dbEntity.Comments,

                Tags = tags
            };

            return viewModel;
        }

        public static UserAskedQuestionsViewModel MapToUserAskedQuestionsViewModel(
            this UserAskedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var tags = dbTags.Select(tag => new TagViewModel
            {
                TagId = tag.TagId,
                TagText = tag.TagText,
                Position = tag.Position
            })
                .ToList();

            var viewModel = new UserAskedQuestionsViewModel
            {
                QuestionId = dbEntity.QuestionId,
                Condition = dbEntity.Condition,
                FirstOption = dbEntity.FirstOption,
                SecondOption = dbEntity.SecondOption,
                QuestionType = dbEntity.QuestionType,
                QuestionAddDate = dbEntity.QuestionAddDate,
                UserId = dbEntity.UserId,
                Login = dbEntity.Login,
                AvatarLink = dbEntity.AvatarLink,
                Answers = dbEntity.Answers,
                Likes = dbEntity.Likes,
                Dislikes = dbEntity.Dislikes,
                YourFeedback = dbEntity.YourFeedback,
                YourAnswer = dbEntity.YourAnswer,
                InFavorites = dbEntity.InFavorites,
                Comments = dbEntity.Comments,

                Tags = tags
            };

            return viewModel;
        }

        public static UserCommentedQuestionsViewModel MapToUserCommentedQuestionsViewModel(
            this UserCommentedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var tags = dbTags.Select(tag => new TagViewModel
                {
                    TagId = tag.TagId,
                    TagText = tag.TagText,
                    Position = tag.Position
                })
                .ToList();


            var viewModel = new UserCommentedQuestionsViewModel
            {
                QuestionId = dbEntity.QuestionId,
                Condition = dbEntity.Condition,
                FirstOption = dbEntity.FirstOption,
                SecondOption = dbEntity.SecondOption,
                QuestionType = dbEntity.QuestionType,
                QuestionAddDate = dbEntity.QuestionAddDate,
                UserId = dbEntity.UserId,
                Login = dbEntity.Login,
                AvatarLink = dbEntity.AvatarLink,
                Answers = dbEntity.Answers,
                Likes = dbEntity.Likes,
                Dislikes = dbEntity.Dislikes,
                YourFeedback = dbEntity.YourFeedback,
                YourAnswer = dbEntity.YourAnswer,
                InFavorites = dbEntity.InFavorites,
                Comments = dbEntity.Comments,

                CommentId = dbEntity.CommentId,
                Comment = dbEntity.Comment,
                CommentLikes = dbEntity.CommentLikes,
                CommentDislikes = dbEntity.CommentDislikes,
                YourCommentFeedback = dbEntity.YourFeedback,
                PreviousCommentId = dbEntity.PreviousCommentId,
                CommentAddDate = dbEntity.CommentAddDate,

                Tags = tags
            };

            return viewModel;
        }

        public static UserFavouriteQuestionsViewModel MapToUserFavouriteQuestionsViewModel(
            this UserFavouriteQuestionsDb dbEntity, IEnumerable<TagDb> dbTags)
        {
            var tags = dbTags.Select(tag => new TagViewModel
            {
                TagId = tag.TagId,
                TagText = tag.TagText,
                Position = tag.Position
            })
                .ToList();

            var viewModel = new UserFavouriteQuestionsViewModel
            {
                QuestionId = dbEntity.QuestionId,
                Condition = dbEntity.Condition,
                FirstOption = dbEntity.FirstOption,
                SecondOption = dbEntity.SecondOption,
                QuestionType = dbEntity.QuestionType,
                QuestionAddDate = dbEntity.QuestionAddDate,
                UserId = dbEntity.UserId,
                Login = dbEntity.Login,
                AvatarLink = dbEntity.AvatarLink,
                Answers = dbEntity.Answers,
                Likes = dbEntity.Likes,
                Dislikes = dbEntity.Dislikes,
                YourFeedback = dbEntity.YourFeedback,
                YourAnswer = dbEntity.YourAnswer,
                InFavorites = dbEntity.InFavorites,
                Comments = dbEntity.Comments,

                Tags = tags
            };

            return viewModel;
        }
    }
}
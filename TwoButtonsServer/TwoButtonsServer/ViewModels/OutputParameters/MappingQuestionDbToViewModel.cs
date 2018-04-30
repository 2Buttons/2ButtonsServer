using System.Collections.Generic;
using System.Linq;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.NewsQuestions;
using TwoButtonsDatabase.Entities.UserQuestions;
using TwoButtonsServer.ViewModels.OutputParameters.NewsQuestions;
using TwoButtonsServer.ViewModels.OutputParameters.UserQuestions;

namespace TwoButtonsServer.ViewModels.OutputParameters
{
    public static class MappingQuestionDbToViewModel
    {

        public static NewsAnsweredQuestionViewModel MapToNewsAnsweredQuestionsViewModel(
            this NewsAnsweredQuestionsDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<NewsAnsweredQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);
            viewModel.AnsweredFollowToAmount = dbEntity.AnsweredFollowTo;
            viewModel.AnswerDate = dbEntity.AnswerDate;
            return viewModel;
        }

        public static NewsAskedQuestionViewModel MapToNewsAskedQuestionsViewModel(
            this NewsAskedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            return QuestionDbToViewModel<NewsAskedQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);
        }

        public static NewsCommentedQuestionViewModel MapToNewsCommentedQuestionsViewModel(
            this NewsCommentedQuestionsDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<NewsCommentedQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);

            viewModel.CommentUserId = dbEntity.CommentUserId;
            viewModel.CommentUserLogin = dbEntity.CommentUserLogin;
            viewModel.UserCommentsAmount = dbEntity.CommentsAmount;
            viewModel.CommentAddDate = dbEntity.CommentAddDate;
            return viewModel;
        }

        public static NewsFavoriteQuestionViewModel MapToNewsFavoriteQuestionsViewModel(
            this NewsFavoriteQuestionsDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<NewsFavoriteQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);
            viewModel.FavoriteAddedUserId = dbEntity.FavoriteAddedUserId;
            viewModel.FavoriteAddedUserLogin = dbEntity.FavoriteAddedUserLogin;
            viewModel.FavoriteAddDate = dbEntity.FavoriteAddDate;
            return viewModel;
        }


        public static NewsRecommendedQuestionViewModel MapNewsRecommendedQuestionsViewModel(
            this NewsRecommendedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<NewsRecommendedQuestionViewModel>(dbEntity, dbTags, dbFirstPhotos,
                    dbSecondPhotos, dbComments);
            viewModel.RecommendedUserId = dbEntity.RecommendedUserId;
            viewModel.RecommendedUserLogin = dbEntity.RecommendedUserLogin;
            viewModel.RecommendedDate = dbEntity.RecommendedDate;
            return viewModel;
        }


        public static UserAnsweredQuestionsViewModel MapToUserAnsweredQuestionsViewModel(
            this UserAnsweredQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<UserAnsweredQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);
            return viewModel;
        }

        public static UserAskedQuestionsViewModel MapToUserAskedQuestionsViewModel(
            this UserAskedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<UserAskedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);
            return viewModel;
        }

        public static UserCommentedQuestionsViewModel MapToUserCommentedQuestionsViewModel(
            this UserCommentedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<UserCommentedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);

            viewModel.CommentId = dbEntity.CommentId;
            viewModel.CommentText = dbEntity.Comment;
            viewModel.LikesAmount = dbEntity.CommentLikes;
            viewModel.DislikesAmount = dbEntity.CommentDislikes;
            viewModel.YourCommentFeedbackType = (FeedbackType)dbEntity.YourFeedback;
            viewModel.PreviousCommentId = dbEntity.PreviousCommentId.GetValueOrDefault(0);
            viewModel.AddDate = dbEntity.CommentAddDate;
            return viewModel;
        }

        public static UserFavoriteQuestionsViewModel MapToUserFavoriteQuestionsViewModel(
            this UserFavoriteQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<UserFavoriteQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);
            return viewModel;
        }


        public static TopQuestionsViewModel MapToTopQuestionsViewModel(
            this TopQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<TopQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);
            return viewModel;
        }


        public static AskedQuestionsViewModel MapToAskedQuestionsViewModel(
            this AskedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<AskedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);
            return viewModel;
        }

        public static LikedQuestionsViewModel MapToLikedQuestionsViewModel(
            this LikedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<LikedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);
            return viewModel;
        }

        public static SavedQuestionsViewModel MapToSavedQuestionsViewModel(
            this SavedQuestionDb dbEntity, IEnumerable<TagDb> dbTags, IEnumerable<PhotoDb> dbFirstPhotos,
            IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
        {
            var viewModel =
                QuestionDbToViewModel<SavedQuestionsViewModel>(dbEntity, dbTags, dbFirstPhotos, dbSecondPhotos, dbComments);
            return viewModel;
        }


        public static T QuestionDbToViewModel<T>(this QuestionBaseDb dbEntity, IEnumerable<TagDb> dbTags,
            IEnumerable<PhotoDb> dbFirstPhotos, IEnumerable<PhotoDb> dbSecondPhotos, IEnumerable<CommentDb> dbComments)
            where T : QuestionBaseViewModel, new()
        {
            var question = new T
            {
                QuestionId = dbEntity.QuestionId,
                Condition = dbEntity.Condition,
                FirstOption = dbEntity.FirstOption,
                SecondOption = dbEntity.SecondOption,
                BackgroundImageLink = dbEntity.BackgroundImageLink,
                QuestionType = (QuestionType)dbEntity.QuestionType,
                AddDate = dbEntity.QuestionAddDate,
                UserId = dbEntity.UserId,
                Login = dbEntity.Login,
                SmallAvatarLink = dbEntity.SmallAvatarLink,
                ShowsAmount = dbEntity.Shows,
                LikesAmount = dbEntity.Likes,
                DislikesAmount = dbEntity.Dislikes,
                YourFeedbackType = (FeedbackType)dbEntity.YourFeedback,
                YourAnswerType = (AnswerType)dbEntity.YourAnswer,
                IsInFavorites = dbEntity.InFavorites==1,
                CommentsAmount = dbEntity.Comments,

                FirstAnswersAmount = dbEntity.FirstAnswers,
                SecondAnswersAmount = dbEntity.SecondAnswers,

                Tags = dbTags.MapTagsDbToTagViewModel(),
                FirstPhotos = dbFirstPhotos.MapPhotosDbToViewModel(),
                SecondPhotos = dbSecondPhotos.MapPhotosDbToViewModel(),
                Comments = dbComments.MapCommentsDbToViewModel()
            };
            return question;
        }

        public static List<TagViewModel> MapTagsDbToTagViewModel(this IEnumerable<TagDb> dbTags)
        {
            return dbTags.Select(tag => new TagViewModel
            {
                TagId = tag.TagId,
                TagText = tag.TagText,
               // Position = tag.Position
            })
                .ToList();
        }

        public static List<PhotoViewModel> MapPhotosDbToViewModel(this IEnumerable<PhotoDb> photosDb)
        {
            return photosDb.Select(p => new PhotoViewModel
            {
                UserId = p.UserId,
                Age = p.Age,
                Login = p.Login,
                SexType = (SexType)p.Sex,
                SmallAvatarLink = p.SmallAvatarLink
            })
                .ToList();
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
                YourFeedbackType = (FeedbackType)c.YourFeedback,
                PreviousCommentId = c.PreviousCommentId.GetValueOrDefault(0),
                AddDate = c.CommentAddDate,
            })
                .ToList();
        }


      public static List<AnsweredListViewModel> MapAnsweredListDbToViewModel(this IEnumerable<AnsweredListDb> answeredList)
      {
        return answeredList.Select(v => new AnsweredListViewModel
          {
            UserId = v.UserId,
            Login= v.Login,
            SmallAvatarLink = v.SmallAvatarLink,
            Age = v.Age,
            SexType = (SexType)v.Sex,
            IsHeFollowed = v.HeFollowed,
            IsYouFollowed = v.YouFollowed,
        })
          .ToList();
      }
  }
}


﻿using System.Collections.Generic;
using System.Linq;
using CommonLibraries;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.Followers;


namespace TwoButtonsServer.ViewModels.OutputParameters
{
    public static class MappingFollowerDbToViewModel
    {
        //public static UserInfoViewModel MapToUserInfoViewModel(
        //    this UserInfoDb dbEntity, UserStatisticsDb statisticsDb, IEnumerable<UserContactsDb> userContactsDb)
        //{
        //    var viewModel = new UserInfoViewModel
        //    {
        //        UserId = dbEntity.UserId,
        //        Login = dbEntity.Login,
        //        Age = dbEntity.Age,
        //        Sex = dbEntity.Sex,
        //        Description = dbEntity.Description,
        //        FullAvatarLink = dbEntity.FullAvatarLink,
        //        IsYouFollowed = dbEntity.YouFollowed ==1,
        //        IsHeFollowed = dbEntity.HeFollowed==1,
        //        AskedQuestionsAmount = dbEntity.AskedQuestions,
        //        AnswersAmount = dbEntity.Answers,
        //        FollowersAmount = dbEntity.Followers,
        //        FollowedAmount = dbEntity.Followed,
        //        FavoritesAmount = dbEntity.Favorites,
        //        CommentsAmount = dbEntity.Comments,

        //        UserStatistics = statisticsDb.MapToUserStatisticsViewModel(),

        //        Social = userContactsDb.MapToUserContactsViewModel()
        //    };
        //    return viewModel;
        //}

        //public static UserStatisticsViewModel MapToUserStatisticsViewModel(
        //    this UserStatisticsDb dbEntity)
        //{
        //    var viewModel = new UserStatisticsViewModel
        //    {
        //        PublicAskedQuestions = dbEntity.PublicAskedQuestions,
        //        AskedQuestions = dbEntity.AskedQuestions,
        //        AnsweredQuestions = dbEntity.AnsweredQuestions,
        //        SeenQuestions = dbEntity.SeenQuestions,
        //        PublicFavoriteQuestions = dbEntity.PublicFavoriteQuestions,
        //        FavoriteQuestions = dbEntity.FavoriteQuestions,
        //        CommentsWritten = dbEntity.CommentsWritten,

        //        UserQuestionsShows = dbEntity.UserQuestionsShows,
        //        UserQuestionsAnswers = dbEntity.UserQuestionsAnswers,

        //        QuestionsCommentsGot = dbEntity.QuestionsCommentsGot,
        //        QuestionsLikesGot = dbEntity.QuestionsLikesGot,
        //        QuestionsDislikesGot = dbEntity.QuestionsDislikesGot,
        //        CommentsLikesGot = dbEntity.CommentsLikesGot,
        //        CommentsDislikesGot = dbEntity.CommentsDislikesGot,
        //        QuestionsLikesMade = dbEntity.QuestionsLikesMade,
        //        QuestionsDislikesMade = dbEntity.QuestionsDislikesMade,
        //        CommentsLikesMade = dbEntity.CommentsLikesMade,
        //        CommentsDislikesMade = dbEntity.CommentsDislikesMade
        //    };
        //    return viewModel;
        //}

        public static List<GetFollowerViewModel> MapToUserContactsViewModel(this IEnumerable<FollowerDb> userContactsDb)
        {
            return userContactsDb.Select(f => new GetFollowerViewModel
            {
              UserId = f.UserId,
              Login = f.Login,
              SmallAvatarLink = f.SmallAvatarLink,
              Age = f.Age,
              SexType = f.Sex,
              VisitsAmount = f.Visits,
              IsYouFollowed = f.YouFollowed,
              IsHeFollowed = f.HeFollowed,
            })
                .ToList();
        }

    }
}


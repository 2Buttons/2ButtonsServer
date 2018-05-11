﻿using System.Collections.Generic;
using System.Linq;
using AccountServer.Helpers;
using AccountServer.Models;
using AccountServer.ViewModels.OutputParameters.User;
using CommonLibraries;
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
        Age = dbEntity.Age,
        SexType = (SexType) dbEntity.Sex,
        Description = dbEntity.Description,
        FullAvatarLink = dbEntity.FullAvatarLink,
        SmallAvatarLink = dbEntity.SmallAvatarLink,
        IsYouFollowed = dbEntity.YouFollowed == 1,
        IsHeFollowed = dbEntity.HeFollowed == 1,

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
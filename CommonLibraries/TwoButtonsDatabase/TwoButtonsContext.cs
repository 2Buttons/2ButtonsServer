﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.Account;
using TwoButtonsDatabase.Entities.Followers;
using TwoButtonsDatabase.Entities.Moderators;
using TwoButtonsDatabase.Entities.NewsQuestions;
using TwoButtonsDatabase.Entities.Recommended;
using TwoButtonsDatabase.Entities.UserQuestions;


namespace TwoButtonsDatabase
{
  public partial class TwoButtonsContext : DbContext
  {
    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options) { }

    //for functions and prosedures
    public virtual DbSet<QuestionDb> QuestionDb { get; set; }
    public virtual DbSet<ComplaintDb> ComplaintDb { get; set; }
    public virtual DbSet<TagDb> TagDb { get; set; }
    public virtual DbSet<AnsweredListDb> AnsweredListDb { get; set; }
    public virtual DbSet<CommentDb> CommentDb { get; set; }

    public virtual DbSet<FollowerDb> FollowerDb { get; set; }
    public virtual DbSet<FollowToDb> FolloToDb { get; set; }
    public virtual DbSet<NewFollowersDb> NewFollowersDb { get; set; }

    public virtual DbSet<RecommendedFromContactsDb> RecommendedFromContactsDb { get; set; }
    public virtual DbSet<RecommendedStrangersDb> RecommendedStrangersDb { get; set; }

    public virtual DbSet<RecommendedFromFollowsDb> RecommendedFromFollowsDb { get; set; }
    public virtual DbSet<RecommendedFromFollowersDb> RecommendedFromFollowersDb { get; set; }
    public virtual DbSet<RecommendedFromUsersIdDb> RecommendedFromUsersIdsDb { get; set; }

    public virtual DbSet<NotificationDb> NotificationsDb { get; set; }

    public virtual DbSet<UserAnsweredQuestionDb> UserAnsweredQuestionsDb { get; set; }
    public virtual DbSet<UserAskedQuestionDb> UserAskedQuestionsDb { get; set; }
    public virtual DbSet<UserCommentedQuestionDb> UserCommentedQuestionsDb { get; set; }
    public virtual DbSet<UserFavoriteQuestionDb> UserFavoriteQuestionsDb { get; set; }

    public virtual DbSet<NewsAnsweredQuestionsDb> NewsAnsweredQuestionsDb { get; set; }
    public virtual DbSet<NewsAskedQuestionsDb> NewsAskedQuestionsDb { get; set; }
    public virtual DbSet<NewsCommentedQuestionsDb> NewsCommentedQuestionsDb { get; set; }
    public virtual DbSet<NewsFavoriteQuestionsDb> NewsFavoriteQuestionsDb { get; set; }
    public virtual DbSet<NewsRecommendedQuestionDb> NewsRecommendedQuestionDb { get; set; }

    public virtual DbSet<AskedQuestionDb> AskedQuestionsDb { get; set; }
    public virtual DbSet<SavedQuestionDb> SavedQuestionsDb { get; set; }
    public virtual DbSet<RecommendedQuestionDb> RecommendedQuestionsDb { get; set; }
    public virtual DbSet<LikedQuestionDb> LikedQuestionsDb { get; set; }
    public virtual DbSet<TopQuestionDb> TopQuestionsDb { get; set; }

    public virtual DbSet<UserInfoDb> UserInfoDb { get; set; }
    public virtual DbSet<UserStatisticsDb> UserStatisticsDb { get; set; }
    public virtual DbSet<PhotoDb> PhotoDb { get; set; }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseSqlServer(@"Server=DESKTOP-T41QO6T\SQLEXPRESS;database=TwoButtons;Trusted_Connection=True;");
    //    }
    //}


  }
}

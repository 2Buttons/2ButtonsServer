using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TwoButtonsDatabase.Entities;
using TwoButtonsDatabase.Entities.ForScalarFunctions;

namespace TwoButtonsDatabase
{
    public partial class TwoButtonsContext : DbContext
    {
        public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options) { }

        //for functions and prosedures
        public virtual DbSet<AnsweredFollowToAndStrangersDb> AnsweredFollowToAndStrangersDb { get; set; }
        public virtual DbSet<AnsweredListDb> AnsweredListDb { get; set; }
        public virtual DbSet<CommentDb> CommentDb { get; set; }
        public virtual DbSet<FollowerDb> FollowerDb { get; set; }
        public virtual DbSet<NewFollowersDb> NewFollowersDb { get; set; }
        public virtual DbSet<NewRecommendedQuestionDb> NewRecommendedQuestionDb { get; set; }
        public virtual DbSet<NewsDb> NewsDb { get; set; }
        public virtual DbSet<PhotoDb> PhotoDb { get; set; }
        public virtual DbSet<PostDb> PostDb { get; set; }
        public virtual DbSet<QuestionDb> QuestionDb { get; set; }
        public virtual DbSet<RecommendedFromFollowsDb> RecommendedFromFollowsDb { get; set; }
        public virtual DbSet<RecommendedFromContactsDb> RecommendedFromContactsDb { get; set; }
        public virtual DbSet<RecommendedStrangersDb> RecommendedStrangersDb { get; set; }
        public virtual DbSet<ResultFollowersPhotosDb> ComResultFollowersPhotosDbments { get; set; }
        public virtual DbSet<ResultFollowToPhotoDb> ResultFollowToPhotoDb { get; set; }
        public virtual DbSet<ResultsDb> ResultsDb { get; set; }
        public virtual DbSet<StrangersPhotosDb> StrangersPhotosDb { get; set; }
        public virtual DbSet<TagDb> TagDb { get; set; }
        public virtual DbSet<TopDb> TopDb { get; set; }
        public virtual DbSet<UserAnsweredQuestionsDb> UserAnsweredQuestionsDb { get; set; }
        public virtual DbSet<UserAskedQuestionsDb> UserAskedQuestionsDb { get; set; }
        public virtual DbSet<UserCommentedQuestionsDb> UserCommentedQuestionsDb { get; set; }
        public virtual DbSet<UserFavouriteQuestionsDb> UserFavouriteQuestionsDb { get; set; }
        public virtual DbSet<UserInfoDb> UserInfoDb { get; set; }

        //for scalara functions
        public virtual DbSet<IdentificationDb> IdentificationDb { get; set; }
        public virtual DbSet<CheckValidLoginDb> CheckValidLoginDb { get; set; }
        public virtual DbSet<CheckValidUserDb> CheckValidUserDb { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Server=DESKTOP-T41QO6T\SQLEXPRESS;database=TwoButtons;Trusted_Connection=True;");
        //    }
        //}


    }
}

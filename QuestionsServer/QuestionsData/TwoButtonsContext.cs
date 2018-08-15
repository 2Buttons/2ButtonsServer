using CommonLibraries.Entities.Main;
using Microsoft.EntityFrameworkCore;
using QuestionsData.Queries;
using QuestionsData.Queries.Moderators;
using QuestionsData.Queries.NewsQuestions;
using QuestionsData.Queries.UserQuestions;

namespace QuestionsData
{
  public class TwoButtonsContext : DbContext
  {
    public virtual DbSet<UserInfoEntity> UserInfoEntities { get; set; }
    public virtual DbSet<CityEntity> CityEntities { get; set; }
    public virtual DbSet<AnswerEntity> AnswerEntities { get; set; }
    public virtual DbSet<QuestionEntity> QuestionEntities { get; set; }
    public virtual DbSet<OptionEntity> OptionEntities { get; set; }
    public virtual DbSet<FollowingEntity> FollowingEntities { get; set; }
    public virtual DbSet<QuestionTagEntity> QuestionTagEntities { get; set; }
    public virtual DbSet<TagEntity> TagEntities { get; set; }
    public virtual DbSet<RecommendedQuestionEntity> RecommendedQuestionEntities { get; set; }
    public virtual DbSet<StatisticsEntity> StatisticsEntities { get; set; }

    //for functions and procedures
    public virtual DbQuery<QuestionDb> QuestionDb { get; set; }

    public virtual DbQuery<QuestionIdDb> QuestionIdDb { get; set; }
    public virtual DbQuery<ComplaintDb> ComplaintDb { get; set; }
    public virtual DbQuery<TagDb> TagDb { get; set; }
    public virtual DbQuery<AnsweredListDb> AnsweredListDb { get; set; }
    public virtual DbQuery<CommentDb> CommentDb { get; set; }

    public virtual DbQuery<UserAnsweredQuestionDb> UserAnsweredQuestionsDb { get; set; }
    public virtual DbQuery<UserAskedQuestionDb> UserAskedQuestionsDb { get; set; }
    public virtual DbQuery<UserCommentedQuestionDb> UserCommentedQuestionsDb { get; set; }
    public virtual DbQuery<UserFavoriteQuestionDb> UserFavoriteQuestionsDb { get; set; }

    public virtual DbQuery<NewsAnsweredQuestionDb> NewsAnsweredQuestionsDb { get; set; }
    public virtual DbQuery<NewsAskedQuestionDb> NewsAskedQuestionsDb { get; set; }
    public virtual DbQuery<NewsCommentedQuestionDb> NewsCommentedQuestionsDb { get; set; }
    public virtual DbQuery<NewsFavoriteQuestionDb> NewsFavoriteQuestionsDb { get; set; }
    public virtual DbQuery<NewsRecommendedQuestionDb> NewsRecommendedQuestionsDb { get; set; }

    public virtual DbQuery<AskedQuestionDb> AskedQuestionsDb { get; set; }
    public virtual DbQuery<SavedQuestionDb> SavedQuestionsDb { get; set; }
    public virtual DbQuery<RecommendedQuestionDb> RecommendedQuestionsDb { get; set; }
    public virtual DbQuery<SelectedQuestionDb> SelectedQuestionsDb { get; set; }
    public virtual DbQuery<LikedQuestionDb> LikedQuestionsDb { get; set; }
    public virtual DbQuery<TopQuestionDb> TopQuestionsDb { get; set; }

    public virtual DbQuery<RecommendedUserQuestionQuery> RecommendedUserQuestionQueries { get; set; }
    public virtual DbQuery<QuestionIdQuery> QuestionIdQueries { get; set; }
    public virtual DbQuery<PhotoDb> PhotoDb { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<AnswerEntity>().HasKey(x => new {x.UserId, x.QuestionId});
      modelBuilder.Entity<FollowingEntity>().HasKey(x => new {x.UserId, x.FollowingId});
      modelBuilder.Entity<RecommendedQuestionEntity>().HasKey(x => new {x.UserFromId, x.UserToId, x.QuestionId});
      modelBuilder.Entity<QuestionTagEntity>().HasKey(x => new {x.QuestionId, x.TagId});

    }
  }
}
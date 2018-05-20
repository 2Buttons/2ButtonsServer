using Microsoft.EntityFrameworkCore;
using QuestionsData.Entities;
using QuestionsData.Entities.Moderators;
using QuestionsData.Entities.NewsQuestions;
using QuestionsData.Entities.UserQuestions;

namespace QuestionsData
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

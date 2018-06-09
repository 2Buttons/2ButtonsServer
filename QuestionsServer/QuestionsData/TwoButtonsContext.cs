using Microsoft.EntityFrameworkCore;
using QuestionsData.Entities;
using QuestionsData.Queries;
using QuestionsData.Queries.Moderators;
using QuestionsData.Queries.NewsQuestions;
using QuestionsData.Queries.UserQuestions;

namespace QuestionsData
{
  public partial class TwoButtonsContext : DbContext
  {
    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options) { }

    

    public virtual DbSet<UserEntity> UserEntities { get; set; }
    public virtual DbSet<CityEntity> CityEntities { get; set; }
    public virtual DbSet<AnswerEntity> AnswerEntities { get; set; }
    public virtual DbSet<QuestionEntity> QuestionEntities { get; set; }

    //for functions and prosedures
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
    public virtual DbQuery<ChosenQuestionDb> ChosenQuestionsDb { get; set; }
    public virtual DbQuery<LikedQuestionDb> LikedQuestionsDb { get; set; }
    public virtual DbQuery<TopQuestionDb> TopQuestionsDb { get; set; }

    public virtual DbQuery<PhotoDb> PhotoDb { get; set; }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseSqlServer(@"Server=DESKTOP-T41QO6T\SQLEXPRESS;database=TwoButtons;Trusted_Connection=True;");
    //    }
    //}


  }
}

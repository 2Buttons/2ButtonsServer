namespace DataGenerator.ScriptsGenerators.Entities
{
  public class StatisticsEntity
  {
    public int UserId { get; set; }
    public int PublicAskedQuestions { get; set; }
    public int AskedQuestions { get; set; } = 0;
    public int PublicAnsweredQuestions { get; set; } = 0;
    public int AnsweredQuestions { get; set; } = 0;
    public int SeenQuestions { get; set; } = 0;
    public int PublicFavoriteQuestions { get; set; } = 0;
    public int FavoriteQuestions { get; set; } = 0;
    public int CommentsWritten { get; set; } = 0;
    public int UserQuestionsShows { get; set; } = 0;
    public int UserQuestionsAnswers { get; set; } = 0;
    public int Followers { get; set; } = 0;
    public int FollowTo { get; set; } = 0;
    public int QuestionsCommentsGot { get; set; } = 0;
    public int QuestionsLikesGot { get; set; } = 0;
    public int QuestionsDislikesGot { get; set; } = 0;
    public int CommentsLikesGot { get; set; } = 0;
    public int CommentsDislikesGot { get; set; } = 0;
    public int QuestionsLikesMade { get; set; } = 0;
    public int QuestionsDislikesMade { get; set; } = 0;
    public int CommentsLikesMade { get; set; } = 0;
    public int CommentsDislikesMade { get; set; } = 0;
  }
}
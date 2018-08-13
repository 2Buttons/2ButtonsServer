using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Entities.Main
{
  [Table("Statistics")]
  public class StatisticsEntity
  {
    [Key]
    public int UserId { get; set; }

    public int PublicAskedQuestions { get; set; }
    public int AskedQuestions { get; set; }

    public int PublicAnsweredQuestions { get; set; }
    public int AnsweredQuestions { get; set; }

    public int SeenQuestions { get; set; }
    public int PublicFavoriteQuestions { get; set; }
    public int FavoriteQuestions { get; set; }
    public int CommentsWritten { get; set; }
    public int UserQuestionsShows { get; set; }
    public int UserQuestionsAnswers { get; set; }

    public int Followers { get; set; }
    public int Followings { get; set; }

    public int QuestionsCommentsGot { get; set; }
    public int QuestionsLikesGot { get; set; }
    public int QuestionsDislikesGot { get; set; }
    public int CommentsLikesGot { get; set; }
    public int CommentsDislikesGot { get; set; }
    public int QuestionsLikesMade { get; set; }
    public int QuestionsDislikesMade { get; set; }
    public int CommentsLikesMade { get; set; }
    public int CommentsDislikesMade { get; set; }
  }
}
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace QuestionsData.Queries.NewsQuestions
{
  public class NewsCommentedQuestionDb : NewsQuestionBaseDb
  {
    public int CommentUserId { get; set; }
    public string CommentUserLogin { get; set; }
    [Column("commentUserSex")]
    public SexType CommentUserSexType { get; set; }
    public int CommentsAmount { get; set; }
  }
}
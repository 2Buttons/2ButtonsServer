using CommonLibraries;

namespace QuestionsData.Queries.NewsQuestions
{
  public class NewsCommentedQuestionDb : NewsQuestionBaseDb
  {
    public int CommentedUserId { get; set; }
    public string CommentedUserFirstName { get; set; }
    public string CommentedUserLastName { get; set; }
    public SexType CommentedUserSexType { get; set; }
    public int CommentedUserCommentsCount { get; set; }
  }
}
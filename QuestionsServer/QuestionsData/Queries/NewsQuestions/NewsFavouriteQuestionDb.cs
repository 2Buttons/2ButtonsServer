using CommonLibraries;

namespace QuestionsData.Queries.NewsQuestions
{
  public class NewsFavoriteQuestionDb : NewsQuestionBaseDb
  {
    public int FavoriteAddedUserId { get; set; }
    public string FavoriteAddedUserFirstName { get; set; }
    public string FavoriteAddedUserLastName { get; set; }
    public SexType FavoriteAddedUserSexType { get; set; }
  }
}
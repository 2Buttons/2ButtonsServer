using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace QuestionsData.Queries.NewsQuestions
{
  public class NewsFavoriteQuestionDb : NewsQuestionBaseDb
  {
    public int FavoriteAddedUserId { get; set; }
    public string FavoriteAddedUserLogin { get; set; }
    [Column("favoriteAddedUserSex")]
    public SexType FavoriteAddedSexType { get; set; }
  }
}
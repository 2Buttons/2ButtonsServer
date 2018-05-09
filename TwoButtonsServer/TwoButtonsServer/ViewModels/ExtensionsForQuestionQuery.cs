using System;
using System.Linq.Expressions;
using TwoButtonsDatabase.Entities;
using TwoButtonsServer.ViewModels.OutputParameters;

namespace TwoButtonsServer.ViewModels
{
  public static class ExtensionsForQuestionQuery
  {
    public static Expression<Func<T, object>> ToPredicate<T>(this SortType type) where T: QuestionBaseDb, new()
    {
      switch (type)
      {
        case SortType.LikesAmount:
          return x => x.Likes;
        case SortType.DislikesAmount:
          return x => x.Dislikes;
        case SortType.ShowsAmount:
          return x => x.Shows;
        case SortType.DateTime:
        default:
          return x => x.QuestionAddDate;
      }
    }

    public static Type ToValueType(this SortType type)

    {
      switch (type)
      {
        case SortType.LikesAmount:
        case SortType.DislikesAmount:
        case SortType.ShowsAmount:
          return typeof(int);
        case SortType.DateTime:
        default:
          return typeof(DateTime);
      }
    }
  }
}
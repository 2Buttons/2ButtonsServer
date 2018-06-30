using System;
using System.Linq.Expressions;
using CommonLibraries;
using QuestionsData.Entities;
using QuestionsData.Queries;

namespace QuestionsServer.Extensions
{
  public static class SortTypeExtentions
  {
    public static Expression<Func<T, object>> ToPredicate<T>(this SortType type) where T: QuestionBaseDb, new()
    {
      switch (type)
      {
        case SortType.Raiting:
          return x => x.Likes-x.Dislikes;
        case SortType.AnswersAmount:
          return x => x.FirstAnswers+x.SecondAnswers;
        case SortType.DateTime:
        default:
          return x => x.QuestionAddDate;
      }
    }


    public static Type ToValueType(this SortType type)

    {
      switch (type)
      {
        case SortType.Raiting:
        case SortType.AnswersAmount:
          return typeof(int);
        case SortType.DateTime:
        default:
          return typeof(DateTime);
      }
    }
  }
}
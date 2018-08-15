using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;

namespace QuestionsServer.ViewModels.OutputParameters
{
    public class EmptyQuestionViewModel
    {
      public int QuestionId { get; set; }
      public string Condition { get; set; }
      public List<Option> Options { get; set; }
      public string BackgroundUrl { get; set; }
      public QuestionType QuestionType { get; set; }
      public DateTime AddedDate { get; set; }
      public AuthorViewModel Author { get; set; }
      public int LikesCount { get; set; }
      public int DislikesCount { get; set; }
      public int CommentsCount { get; set; }
  }
}

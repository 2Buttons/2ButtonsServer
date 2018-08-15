using System;
using System.Collections.Generic;
using System.Text;
using CommonLibraries;

namespace QuestionsData.DTO
{
  public class EmptyQuestionDto
  {
    public int QuestionId { get; set; }
    public string Condition { get; set; }
    public List<OptionDto> Options { get; set; }
    public string OriginalBackgroundUrl { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime AddedDate { get; set; }
    public AuthorDto Author { get; set; }
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }
    public int CommentsCount { get; set; }
  }
}

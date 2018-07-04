using System;
using CommonLibraries;

namespace DataGenerator.ScriptsGenerators.Entities
{
  public class AnswerEntity
  {
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    public AnswerType AnswerType { get; set; }
    public bool IsLiked { get; set; }
    public DateTime AnswerDate { get; set; }
    public bool IsDeleted { get; set; }
  }
}
using System;
using CommonLibraries;

namespace DataGenerator.ScriptsGenerators.DirectInsertion.Entities
{
  public class AnswerEntity
  {
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    public AnswerType AnswerType { get; set; }
    public QuestionFeedbackType FeedbackType { get; set; }
    public bool IsFavorite { get; set; }
    public bool IsSaved { get; set; }
    public DateTime AnsweredDate { get; set; }
  }
}
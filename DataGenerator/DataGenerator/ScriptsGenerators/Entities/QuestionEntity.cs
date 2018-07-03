using System;
using CommonLibraries;

namespace DataGenerator.ScriptsGenerators.Entities
{
  public class QuestionEntity
  {
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public string Condition { get; set; }
    public bool IsAnonimoty { get; set; }
    public AudienceType AudienceType { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime QuestionAddDate { get; set; }
    public string BackgroundImageLink { get; set; }
    public int Shows { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public bool IsDeleted { get; set; }
  }
}
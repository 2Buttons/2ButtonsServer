﻿using System;
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
    public string OriginalBackgroundUrl { get; set; }
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }
    public bool IsDeleted { get; set; }
  }
}
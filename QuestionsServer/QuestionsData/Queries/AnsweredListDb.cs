﻿using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace QuestionsData.Queries
{
  public class AnsweredListDb
  {
    [Key]
    public int UserId { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string OriginalAvatarUrl { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public bool IsHeFollowed { get; set; }
    public bool IsYouFollowed { get; set; }
  }
}
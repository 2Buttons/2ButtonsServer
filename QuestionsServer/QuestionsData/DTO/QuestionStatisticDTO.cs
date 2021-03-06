﻿using System.Collections.Generic;
using CommonLibraries;

namespace QuestionsData.DTO
{
  public class QuestionStatisticDto
  {
    public int Count { get; set; }
    public List<VoterFriendDto> Friends { get; set; }
  }

  public class VoterFriendDto
  {
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public SexType SexType { get; set; }
    public int Age { get; set; }
    public string OriginalAvatarUrl { get; set; }
  }
}
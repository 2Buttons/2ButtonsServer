﻿using System.Collections.Generic;
using QuestionsData.Queries;

namespace QuestionsData.DTO
{
  public class RecommendedQuestionDto : QuestionBaseDb
  {
    public List<RecommendedToUserDto> RecommendedToUsers { get; set; } = new List<RecommendedToUserDto>();
  }

  public class RecommendedToUserDto
  {
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    //public string SmallAvatarUrl { get; set; }
  }
}
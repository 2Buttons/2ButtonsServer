﻿using System;
using System.Collections.Generic;
using System.Text;
using CommonLibraries;

namespace BotsData.Dto
{
  public class QuestionDto
  {
    public int QuestionId { get; set; }
    public string Condition { get; set; }
    public List<OptionDto> Options { get; set; }
    public string BackgroundImageLink { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime QuestionAddDate { get; set; }
    public int UserId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarLink { get; set; }
    public int LikesAmount { get; set; }
    public int DislikesAmount { get; set; }
    public FeedbackType YourFeedbackType { get; set; }
    public AnswerType YourAnswerType { get; set; }
    public bool IsInFavorites { get; set; }
    public bool IsSaved { get; set; }
    public int CommentsAmount { get; set; }
  }
}

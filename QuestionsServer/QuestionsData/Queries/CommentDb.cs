﻿using System;
using CommonLibraries;

namespace QuestionsData.Queries
{
  public class CommentDb
  {
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public string Login { get; set; }
    public string OriginalAvatarUrl { get; set; }
    public string Text { get; set; }
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }
    public QuestionFeedbackType YourFeedbackType { get; set; }
    public int? PreviousCommentId { get; set; }
    public DateTime CommentAddDate { get; set; }
  }
}
﻿namespace QuestionsServer.ViewModels.OutputParameters.NewsQuestions
{
  public class NewsCommentedQuestionViewModel : NewsQuestionBaseViewModel
  {
    public NewsUserViewModel CommentedUser { get; set; }
    public int UserCommentsCount { get; set; }
  }
}
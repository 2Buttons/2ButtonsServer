﻿using System;

namespace QuestionsServer.ViewModels.OutputParameters.UserQuestions
{
  public class RecommendedQuestionViewModel : QuestionBaseViewModel
  {
    public int ToUserId { get; set; }
    public string ToUserLogin { get; set; }
    public DateTime RecommendDate { get; set; }
  }
}
using System;
using System.Collections.Generic;
using QuestionsData.DTO;

namespace QuestionsServer.ViewModels.OutputParameters.UserQuestions
{
  public class RecommendedQuestionViewModel : QuestionBaseViewModel
  {
    public List<RecommendedToUserDto> Users { get; set; }
  }

}
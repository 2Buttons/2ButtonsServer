using System;
using System.Collections.Generic;
using QuestionsData.DTO;

namespace QuestionsServer.ViewModels.OutputParameters.UserQuestions
{
  public class RecommendedQuestionViewModel : QuestionBaseViewModel
  {
    public int ToUserId { get; set; }
    public string ToUserLogin { get; set; }
    public DateTime RecommendDate { get; set; }

    public List<RecommendedToUserDto> Users { get; set; }
  }

}
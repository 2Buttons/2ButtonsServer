using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;

namespace NotificationServer.ViewModels.Input
{
  public class AnswerNotification
  {
    [Required]
    [IdValidation(nameof(NotifierId))]
    public int NotifierId { get; set; }

    [Required]
    [IdValidation(nameof(QuestionId))]
    public int QuestionId { get; set; }

    public AnswerType AnswerType { get; set; }
    public DateTime AnsweredDate { get; set; }
  }
}
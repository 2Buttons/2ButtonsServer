﻿using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries.Validation;

namespace NotificationsServer.ViewModels.Input
{
  public class RecommendedQuestionNotification
  {
    [Required]
    [IdValidation(nameof(NotifierId))]
    public int NotifierId { get; set; }

    [Required]
    [IdValidation(nameof(UserToId))]
    public int UserToId { get; set; }

    [Required]
    [IdValidation(nameof(QuestionId))]
    public int QuestionId { get; set; }

    [Required]
    public DateTime RecommendedDate { get; set; }
  }
}
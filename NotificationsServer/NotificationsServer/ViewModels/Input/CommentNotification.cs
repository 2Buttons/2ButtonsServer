﻿using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries.Validation;

namespace NotificationsServer.ViewModels.Input
{
  public class CommentNotification
  {
    [Required]
    [IdValidation(nameof(NotifierId))]
    public int NotifierId { get; set; }

    [Required]
    [IdValidation(nameof(QuestionId))]
    public int QuestionId { get; set; }

    [Required]
    [IdValidation(nameof(CommentId))]
    public int CommentId { get; set; }

    [Required]
    public DateTime CommentedDate { get; set; }
  }
}
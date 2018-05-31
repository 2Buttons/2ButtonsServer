using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries.Validation;

namespace NotificationServer.ViewModels.Input
{
  public class FollowNotification
  {
    [Required]
    [IdValidation(nameof(NotifierId))]
    public int NotifierId { get; set; }

    [Required]
    [IdValidation(nameof(FollowToId))]
    public int FollowToId { get; set; }

    public DateTime FollowedDate { get; set; }
  }
}
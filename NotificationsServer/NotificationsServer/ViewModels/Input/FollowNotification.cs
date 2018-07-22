using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries.Validation;

namespace NotificationsServer.ViewModels.Input
{
  public class FollowNotification
  {
    [Required]
    [IdValidation(nameof(NotifierId))]
    public int NotifierId { get; set; }

    [Required]
    [IdValidation(nameof(FollowingId))]
    public int FollowingId { get; set; }

    [Required]
    public DateTime FollowedDate { get; set; }
  }
}
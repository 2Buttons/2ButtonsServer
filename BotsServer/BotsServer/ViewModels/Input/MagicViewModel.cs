﻿namespace BotsServer.ViewModels.Input
{
  public class MagicViewModel : ClientIdentity
  {
    public int QuestionId { get; set; }
    public int FirstOptionPercent { get; set; }
    public int VoteDurationInMilliseconds { get; set; }
    public int BotsCount { get; set; }
  }
}
﻿using System;

namespace DataGenerator.ScriptsGenerators.Entities
{
  public class FollowEntity
  {
    public int FollowId { get; set; }
    public int FollowingId { get; set; }
    public int Visits { get; set; }
    public DateTime FollowDate { get; set; }
  }
}
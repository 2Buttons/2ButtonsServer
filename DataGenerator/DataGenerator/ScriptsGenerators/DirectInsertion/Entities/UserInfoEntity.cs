﻿using System;
using CommonLibraries;

namespace DataGenerator.ScriptsGenerators.DirectInsertion.Entities
{
  public class UserInfoEntity
  {
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public int CityId { get; set; }
    public string Description { get; set; }
    public DateTime LastNotsSeenDate { get; set; }
    public string OriginalAvatarUrl { get; set; }
  }
}
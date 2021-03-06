﻿using System;
using CommonLibraries;

namespace DataGenerator.ScriptsGenerators.DirectInsertion.Entities
{

  public  class UserEntity
  {
   
    public int UserId { get; set; }
    //public int AccessFailedCount { get; set; }
    public string Email { get; set; }
   // public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
   // public bool PhoneNumberConfirmed { get; set; }
    public RoleType RoleType { get; set; }
   // public bool TwoFactorEnabled { get; set; }
    public DateTime RegistrationDate { get; set; }
  }
}

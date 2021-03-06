﻿using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonLibraries.Validation;

namespace AccountServer.ViewModels.InputParameters
{
  public class UpdateUserInfoViewModel
  {
    [Required]
    [IdValidation]
    public int UserId { get; set; }

    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
  }
}
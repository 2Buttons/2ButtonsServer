﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace AuthorizationData.Main.Entities
{
  public class UserInfoDb
  {
    [Key]
    public int UserId { get; set; }
    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType Sex { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    [Column("FullAvatarLink")]
    public string LargeAvatarLink { get; set; }
    public string SmallAvatarLink { get; set; }
  }
}
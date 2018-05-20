﻿using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace SocialData.Main.Entities.Recommended
{
  public partial class RecommendedFromFollowersDb
  {
    [Key]
    public int UserId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarLink { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType Sex { get; set; }
    public int CommonFollowsTo { get; set; }
  }
}

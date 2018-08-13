using System;
using System.Collections.Generic;
using System.Text;
using CommonLibraries;

namespace SocialData.Main.DTO
{
    public class RecommendedFromFollowersDto
    {
      public int UserId { get; set; }
      public string Login { get; set; }
      public string OriginalAvatarUrl { get; set; }
      public DateTime BirthDate { get; set; }
      public SexType SexType { get; set; }
      public int CommonFollowingsCount { get; set; }
  }
}

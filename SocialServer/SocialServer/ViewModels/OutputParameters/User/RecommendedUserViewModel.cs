using System;
using CommonLibraries;
using Newtonsoft.Json;

namespace SocialServer.ViewModels.OutputParameters.User
{
  public class RecommendedUserViewModel 
  {
    public int Position { get; set; }

    public int UserId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarUrl { get; set; }
    public int Age { get; set; }
    public SexType SexType { get; set; }

    public int CommonFollowingsCount { get; set; }
  }
}
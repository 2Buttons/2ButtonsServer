using System.Collections.Generic;
using SocialServer.ViewModels.OutputParameters.User;

namespace SocialServer.ViewModels.OutputParameters
{
    public class RecommendedUsers
    {
      public List<RecommendedUserViewModel> SocialNetFrineds = new List<RecommendedUserViewModel>();
      public List<RecommendedUserViewModel> Followers = new List<RecommendedUserViewModel>();
      public List<RecommendedUserViewModel> CommonFollowsTo = new List<RecommendedUserViewModel>();
  }
}

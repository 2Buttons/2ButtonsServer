using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.ViewModels.OutputParameters.User;

namespace AccountServer.ViewModels.OutputParameters
{
    public class RecommendedUsers
    {
      public List<RecommendedUserViewModel> SocialNetFrineds = new List<RecommendedUserViewModel>();
      public List<RecommendedUserViewModel> Followers = new List<RecommendedUserViewModel>();
      public List<RecommendedUserViewModel> CommonFollowsTo = new List<RecommendedUserViewModel>();
  }
}

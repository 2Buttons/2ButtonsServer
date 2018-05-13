using System;
using CommonLibraries;
using Newtonsoft.Json;

namespace AccountServer.ViewModels.OutputParameters.User
{
  public class RecommendedUserViewModel
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarLink { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }

    [JsonIgnore]
    public int CommonFollowsTo { get; set; }
  }
}
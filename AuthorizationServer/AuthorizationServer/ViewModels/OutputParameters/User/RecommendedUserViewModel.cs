using System;
using CommonLibraries;
using CommonTypes;
using Newtonsoft.Json;

namespace AuthorizationServer.ViewModels.OutputParameters.User
{
  public class RecommendedUserViewModel 
  {
    public int Position { get; set; }

    public int UserId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarUrl { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }

    [JsonIgnore]
    public int CommonFollowsTo { get; set; }
  }
}
using System.Collections.Generic;
using CommonLibraries;

namespace QuestionsServer.ViewModels.OutputParameters
{
  public class QuestionStatisticUserViewModel
  {
    public List<List<VoterUserViewModel>> Voters = new List<List<VoterUserViewModel>>();
  }

  public class VoterUserViewModel
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public SexType SexType { get; set; }
    public int Age { get; set; }
    public string SmallAvatarUrl { get; set; }
    public bool IsYouFollowed { get; set; }
    public bool IsHeFollowed { get; set; }
    public string City { get; set; }
  }
}
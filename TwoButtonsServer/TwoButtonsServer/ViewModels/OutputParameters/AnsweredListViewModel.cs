using CommonLibraries;

namespace QuestionsServer.ViewModels.OutputParameters
{
    public class AnsweredListViewModel
    {
      public int UserId { get; set; }
      public string Login { get; set; }
      public string SmallAvatarLink { get; set; }
      public int Age { get; set; }
      public SexType SexType { get; set; }
      public bool IsHeFollowed { get; set; }
      public bool IsYouFollowed { get; set; }
  }
}

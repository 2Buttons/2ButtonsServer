namespace CommonLibraries
{
  public enum ApplicationType
  {
    JavaScript = 0,
    NativeConfidential = 1
  }

  public enum GrantType
  {
    NoGrantType = 0,
    Password = 1,
    RefreshToken = 2
  }


  public enum SexType
  {
    Both = 0,
    Man = 1,
    Woman = 2
  }

  public enum RoleType
  {
    Guest = 0,
    User = 1,
    Moderator = 2,
    Admin = 3
  }

  public enum SocialNetType
  {
    Nothing = 0,
    Facebook = 1,
    Vk = 2,
    Twiter = 3,
    GooglePlus = 4,
    Telegram = 5,
    Badoo = 6
  }


  public enum FeedbackType
  {
    Dislike = -1,
    Neutral = 0,
    Like = 1
  }

  public enum ActionType
  {
    NoType = 0,
    Follow = 1,
    Recommend = 2,
    Answer = 3
  }

  public enum QuestionType
  {
    NoType = 0,
    Opened = 1,
    Anonymous = 2
  }

  public enum AnswerType
  {
    NoAnswer = 0,
    First = 1,
    Second = 2
  }

  public enum Complaintype
  {
    NoType = 0,
    Spam = 1,
    VerbalAbuse = 2,
    RepulsiveContent = 3,
    AdultContent = 4
  }

  public enum SortType
  {
    DateTime = 0,
    Raiting = 1,
    ShowsAmount = 2
  }
}
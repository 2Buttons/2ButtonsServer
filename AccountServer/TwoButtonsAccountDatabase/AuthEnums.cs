namespace TwoButtonsAccountDatabase
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
    VK = 2,
    Twiter = 3,
    GooglePlus = 4,
    Telegram = 5,
    Badoo = 6
  }
}

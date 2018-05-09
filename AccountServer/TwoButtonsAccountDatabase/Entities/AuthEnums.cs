namespace AccountServer.Data.Entities
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
}

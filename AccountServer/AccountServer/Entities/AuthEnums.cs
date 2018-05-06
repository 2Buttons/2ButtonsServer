namespace AccountServer.Entities
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
}

namespace AccountServer.Entities
{
  public enum UserRole
  {
    User = 1,
    Moderator = 2,
    Admin = 3
  }

  public static class EnumHelper
  {
    public static string RoleToString(this UserRole role)
    {
      switch (role)
      {
        case UserRole.User:
          return "User";
        case UserRole.Moderator:
          return "Moderator";
        case UserRole.Admin:
          return "Admin";
        default:
          return "Guest";
      }
    }
  }
}
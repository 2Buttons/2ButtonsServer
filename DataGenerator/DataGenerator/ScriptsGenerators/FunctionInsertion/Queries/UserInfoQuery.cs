using System;
using CommonLibraries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.Queries
{
  public class UserInfoEntityAddUSer
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public string OriginalAvatarUrl { get; set; }
  }
}
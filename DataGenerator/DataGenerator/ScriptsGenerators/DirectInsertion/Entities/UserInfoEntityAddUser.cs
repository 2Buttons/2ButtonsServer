using System;
using CommonLibraries;

namespace DataGenerator.ScriptsGenerators.DirectInsertion.Entities
{
  public class UserInfoEntityAddUSer
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType SexType { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public DateTime LastNotsSeenDate { get; set; }
    public string OriginalAvatarUrl { get; set; }
  }
}
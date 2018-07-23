using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace QuestionsData.Queries
{
  public class PhotoDb
  {
    [Key]
    public long UserId { get; set; }

    public string Login { get; set; }
    public SexType SexType { get; set; }
    public DateTime BirthDate { get; set; }
    public string OriginalAvatarUrl { get; set; }
    public string City { get; set; }
  }
}
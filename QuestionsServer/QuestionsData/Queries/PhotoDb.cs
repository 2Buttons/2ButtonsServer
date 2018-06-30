using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace QuestionsData.Queries
{
  public class PhotoDb
  {
    [Key]
    public int UserId { get; set; }

    public string Login { get; set; }

    [Column("Sex")]
    public SexType SexType { get; set; }

    public DateTime BirthDate { get; set; }
    public string SmallAvatarLink { get; set; }
    public string City { get; set; }
  }
}
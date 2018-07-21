using System;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace AccountData.Main.Queries
{
  public class UserInfoDb
  {
  
    public int UserId { get; set; }

    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    [Column("Sex")]
    public SexType SexType { get; set; }
    public string City { get; set; }
    public string Description { get; set; }

    [Column("fullAvatarUrl")]
    public string OriginalAvatarUrl { get; set; }
    //public string SmallAvatarUrl { get; set; }
    public bool YouFollowed { get; set; }
    public bool HeFollowed { get; set; }

    public int AskedQuestions { get; set; }
    public int Answers { get; set; }
    public int Followers { get; set; }
    public int Followed { get; set; }
    public int Favorites { get; set; }
    public int Comments { get; set; }
  }
}
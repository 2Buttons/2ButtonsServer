using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace AccountData.Main.Entities
{
  public class Info
  {
    [Key]
    public int UserId { get; set; }

    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType Sex { get; set; }
    public string City { get; set; }
    public string Description { get; set; }

    public string FullAvatarLink { get; set; }
    public string SmallAvatarLink { get; set; }
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
using CommonLibraries;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Entities
{
  public class AnsweredListDb
  {
    [Key]
    public int UserId { get; set; }

    public string Login { get; set; }
    public string SmallAvatarLink { get; set; }
    public DateTime BirthDate { get; set; }
    public SexType Sex { get; set; }
    public bool HeFollowed { get; set; }
    public bool YouFollowed { get; set; }
  }
}
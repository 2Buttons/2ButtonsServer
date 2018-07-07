using System;
using System.Collections.Generic;
using System.Text;
using CommonLibraries;

namespace BotsData.Dto
{
  public class QuestionDto
  {
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public string Condition { get; set; }
    public IEnumerable<OptionDto> Options { get; set; } 
    public string BackgroundImageLink { get; set; }
    public AudienceType AudienceType { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime QuestionAddDate { get; set; }
    public int LikesAmount { get; set; }
    public int DislikesAmount { get; set; }
   
  }
}

using System;
using System.Collections.Generic;

namespace DataViewer.ViewModels
{
  public class QuestionViewModel
  {
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public string Condition { get; set; }
    public IEnumerable<OptionViewModel> Options { get; set; }
    public string BackgroundImageLink { get; set; }
    public AudienceType AudienceType { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime QuestionAddDate { get; set; }
    public int LikesAmount { get; set; }
    public int DislikesAmount { get; set; }
  }
}
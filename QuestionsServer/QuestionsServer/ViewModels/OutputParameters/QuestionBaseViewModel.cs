using System;
using System.Collections;
using System.Collections.Generic;
using CommonLibraries;

namespace QuestionsServer.ViewModels.OutputParameters
{
  public class QuestionBaseViewModel
  {
    public int QuestionId { get; set; }
    public string Condition { get; set; }
    public List<Option> Options { get; set; }
    public string BackgroundUrl { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime QuestionAddDate { get; set; }
    public AuthorViewModel Author { get; set; }
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }
    public QuestionFeedbackType YourFeedbackType { get; set; }
    public AnswerType YourAnswerType { get; set; }
    public bool IsFavorite { get; set; }
    public bool IsSaved { get; set; }
    public int CommentsCount { get; set; }

    public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

    public List<List<PhotoViewModel>> Photos { get; set; } = new List<List<PhotoViewModel>>();
   // public List<PhotoViewModel> SecondPhotos { get; set; } = new List<PhotoViewModel>();


  }

  public class Option
  {
    public int Voters { get; set; }
    public string Text { get; set; }

    public Option()
    {
    }

    public Option(int voters, string text)
    {
      Voters = voters;
      Text = text;
    }
  }
}

using System;
using System.Collections.Generic;
using CommonLibraries;

namespace QuestionsServer.ViewModels.OutputParameters
{
  public class QuestionBaseViewModel
  {
    public int QuestionId { get; set; }
    public string Condition { get; set; }
    public List<Option> Options { get; set; }
    public string BackgroundImageLink { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime QuestionAddDate { get; set; }
    public int UserId { get; set; }
    public string Login { get; set; }
    public string SmallAvatarLink { get; set; }
    public int LikesAmount { get; set; }
    public int DislikesAmount { get; set; }
    public FeedbackType YourFeedbackType { get; set; }
    public AnswerType YourAnswerType { get; set; }
    public bool IsInFavorites { get; set; }
    public bool IsSaved { get; set; }
    public int CommentsAmount { get; set; }

    public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

    public List<PhotoViewModel> FirstPhotos { get; set; } = new List<PhotoViewModel>();
    public List<PhotoViewModel> SecondPhotos { get; set; } = new List<PhotoViewModel>();
  }

  public class Option
  {
    public int Voters { get; set; }
    public string Text { get; set; }

    public Option() { }

    public Option(int voters, string text)
    {
      Voters = voters;
      Text = text;
    }
  }
}
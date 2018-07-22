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
    public SexType SexType { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public string OriginalAvatarUrl { get; set; }
    public bool IsYouFollowed { get; set; }
    public bool IsHeFollowed { get; set; }

    public int AskedQuestionsCount { get; set; }
    public int AnswersCount { get; set; }
    public int FollowersCount { get; set; }
    public int FollowedCount { get; set; }
    public int FavoritesCount { get; set; }
    public int CommentsCount { get; set; }
  }
}
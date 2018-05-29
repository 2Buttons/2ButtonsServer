using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CommonLibraries
{
  public enum AvatarSizeType
  {
    //[Display(Name = "small")]
    //[Description("small")]
    SmallAvatar = 0,
    //[Display(Name = "large")]
    //[Description("large")]
    LargeAvatar = 1
  }

  public enum BackgroundType
  {
    Background = 0,
    CustomBackground = 1
  }

  public enum ApplicationType
  {
    JavaScript = 0,
    NativeConfidential = 1
  }

  public enum GrantType
  {
    Guest = 0,
    Password = 1,
    Email = 2
  }

  public enum SexType
  {
    Both = 0,
    Man = 1,
    Woman = 2
  }

  public enum RoleType
  {
    Guest = 0,
    User = 1,
    Moderator = 2,
    Admin = 3
  }

  public enum SocialType
  {
    Nothing = 0,
    Facebook = 1,
    Vk = 2,
    Twiter = 3,
    GooglePlus = 4,
    Telegram = 5,
    Badoo = 6
  }

  public enum FeedbackType
  {
    Dislike = -1,
    Neutral = 0,
    Like = 1
  }

  public enum ActionType
  {
    NoType = 0,
    Follow = 1,
    Recommend = 2,
    Answer = 3
  }

  public enum QuestionType
  {
    NoType = 0,
    Opened = 1,
    Anonymous = 2
  }

  public enum AnswerType
  {
    NoAnswer = 0,
    First = 1,
    Second = 2
  }

  public enum ComplaintType
  {
    NoType = 0,
    Spam = 1,
    VerbalAbuse = 2,
    RepulsiveContent = 3,
    AdultContent = 4
  }

  public enum SortType
  {
    DateTime = 0,
    Raiting = 1,
    AnswersAmount = 2
  }

  public enum LanguageType
  {
    Русский = 0,
    Украинский = 1,
    Английский = 3,
    Испанский = 4,
    Португальский = 12,
    Немецкий = 6,
    Французский = 16,
    Итальянский = 7
  }
}
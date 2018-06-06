namespace CommonLibraries
{
  //public enum UrlMonitoringType // Каждые 100 новые области
  //{
  //  QuestionsUserAsked,
  //  QuestionsUserAnswered,
  //  QuestionsUserFavorite,
  //  QuestionsUserCommented,
  //  QuestionsPersonalAsked,
  //  QuestionsPersonalRecommended,
  //  QuestionsPersonalChosen,
  //  QuestionsPersonalLiked,
  //  QuestionsPersonalSaved,
  //  QuestionsPersonalTop,
  //  QuestionsNews,
  //  QuestionsUserAsked,
  //  QuestionsUserAsked,
  //  QuestionsUserAsked, QuestionsUserAsked,


  //}

  public enum AvatarSizeType
  {
    SmallAvatar = 0,
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
    Phone = 1,
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

  public enum QuestionFeedbackType
  {
    Dislike = -1,
    Neutral = 0,
    Like = 1
  }

  public enum FeedbackType
  {
    Other = 0,
    Error = 1,
    Suggestion = 2,
    Complaint = 3
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
    Russian = 0,
    Ukrainian = 1,
    English = 3,
    Spanish = 4,
    Portuguese = 12,
    German = 6,
    French = 16,
    Italian = 7
  }
}
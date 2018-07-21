namespace CommonLibraries
{
  public enum UrlMonitoringType // Каждые 10 новые области
  {
    GetsQuestionsUserAsked,
    GetsQuestionsUserAnswered,
    GetsQuestionsUserFavorite,
    GetsQuestionsUserCommented,

    GetsQuestionsPersonalAsked,
    GetsQuestionsPersonalRecommended,
    GetsQuestionsPersonalChosen,
    GetsQuestionsPersonalLiked,
    GetsQuestionsPersonalSaved,

    GetsQuestionsPersonalDayTop,
    GetsQuestionsPersonalWeekTop,
    GetsQuestionsPersonalMonthTop,
    GetsQuestionsPersonalAllTimeTop,

    GetsQuestionsNews,

    OpensQuestionPage,
    FiltersQuestions,

    GetsComments,

    OpensPersonalPage,
    OpensUserPage,

    GetsNotifications
  }

  public enum NewsType
  {
    Answered,
    Asked,
    Commented,
    Favorite,
    Recommended
  }

  public enum AvatarSizeType
  {
    Original = 0,
    Small = 1,
    Large = 2
  }

  public enum DefaultSizeType
  {
    Original = 0,
    Small = 1,
    Large = 2
  }

  public enum AvatarType
  {
    Standard = 1,
    Custom = 0
  }

  public enum BackgroundType
  {
    Standard = 1,
    Custom = 0
  }

  public enum BackgroundSizeType
  {
    Original = 0,
    Mobile = 1
  }

  public enum MediaType
  {
    None = 0,
    Default = 1,
    Avatar = 2,
    Background = 3
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

  public enum AudienceType
  {
    All = 0,
    Followers = 1
  }
}
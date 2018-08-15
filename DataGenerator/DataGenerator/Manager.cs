using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.MediaFolders.Configurations;
using DataGenerator.Data.Reader;
using DataGenerator.Data.Reader.Objects;
using DataGenerator.Models;
using DataGenerator.ScriptsGenerators.DirectInsertion.Entities;
using DataGenerator.ScriptsGenerators.FunctionInsertion.Queries;
using DataGenerator.ScriptsGenerators.FunctionInsertion.ScriptGenerators;
using DataGenerator.Services;
using Newtonsoft.Json;

namespace DataGenerator
{
  public class Manager
  {
    private const string Mail = "@mockmail.com";

    public const string QuestionsFile = @"Questions.xlsx";
    public const string UsersFile = "Users.txt";
    public const string MainCitiesFile = "MainCities.txt";
    public const string EmailFile = "EnglishWord.txt";

    public const string UsersBag = "Users.txt";
    public const string QuestionsBag = "Questions.txt";
    public const string EmailsBag = "Emails.txt";
    public const string CitiesBag = "Cities.txt";
    public const string CitiesMatching = "CitiesMatching.txt";

    public const string VkUsersUrl = @"E:\Projects\2Buttons\Project\Data\Files\UnpreparedVkUsers\";

    public const string BagsUrl = @"E:\Projects\2Buttons\Project\Data\Bags\";

    public const string FilesUrl = @"E:\Projects\2Buttons\Project\Data\Files\";

    public const string ScriptsUrl = @"E:\Projects\2Buttons\Project\Data\Scripts\";

    private readonly FilesManager _filesManager = new FilesManager();
    private readonly MediaService _mediaService;

    private readonly Random _random = new Random();

    public Manager()
    {
      var folderConfiguration = new FolderConfiguration();
      var fileService = new FileService(folderConfiguration);
      _mediaService = new MediaService(fileService, folderConfiguration);
    }

    public void CreateScripts(List<CityQuery> mainCities, List<UserInfoQuery> userInfos, List<UserQuery> users,
      List<FollowingEntity> followers, List<QuestionQuery> questions, List<TagQuery> tags, List<AnswerEntity> answerEntities
      //List<AnswerQuery> answers,
      //List<QuestionFeedbackQuery> questionFeedbacks
      )
    {
      var path = @"E:\Projects\2Buttons\Project\Data\Scripts\";
      var tagsUrl = "5_Tags.sql";
      var questionsUrl = "4_Questions.sql";
      var questionFeedbacksUrl = "8_QuestionFeedbacks.sql";
      var usersUrl = "2_Users.sql";
      var usersInfoUrl = "3_UserInfos.sql";
      var citiesUrl = "1_MainCities.sql";
      var answersUrl = "6_Answers.sql";
      var followersUrl = "7_Followers.sql";
      var answersAndFeedbacks = "6_AnswersAndFeedbacks.sql";

      using (var sw = new StreamWriter(Path.Combine(path, citiesUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new CityGenerator().GetInsertionLine(mainCities));
      }

      using (var sw = new StreamWriter(Path.Combine(path, usersUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new UserGenerator().GetInsertionLine(users));
      }

      using (var sw = new StreamWriter(Path.Combine(path, usersInfoUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new UserInfoGenerator().GetInsertionLine(userInfos));
      }

      using (var sw = new StreamWriter(Path.Combine(path, tagsUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new TagGenerator().GetInsertionLine(tags));
      }

      using (var sw = new StreamWriter(Path.Combine(path, questionsUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new QuestionGenerator().GetInsertionLine(questions));
      }

      //using (var sw = new StreamWriter(Path.Combine(path, answersUrl), false, Encoding.UTF8))
      //{
      //  sw.WriteLine(new AnswerGenerator().GetInsertionLine(answers));
      //}

      using (var sw = new StreamWriter(Path.Combine(path, followersUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new DataGenerator.ScriptsGenerators.DirectInsertion.FollowerGenerator().GetInsertionLine(followers));
      }

      using (var sw = new StreamWriter(Path.Combine(path, answersAndFeedbacks), false, Encoding.UTF8))
      {
        sw.WriteLine(new DataGenerator.ScriptsGenerators.DirectInsertion.AnswerGenerator().GetInsertionLine(answerEntities));
      }


      //using (var sw = new StreamWriter(Path.Combine(path, questionFeedbacksUrl), false, Encoding.UTF8))
      //{
      //  sw.WriteLine(new QuestionFeedbackGenerator().GetInsertionLine(questionFeedbacks));
      //}
    }

    public List<AnswerEntity> CreateAnswersEntities(List<AnswerQuery> answerQueries,
      List<QuestionFeedbackQuery> feedbackQueries)
    {
      List<AnswerEntity> answerEntities = new List<AnswerEntity>();
      foreach (var answer in answerQueries)
      {
        answerEntities.Add(new AnswerEntity
        {
          AnsweredDate = answer.AnsweredDate,
          AnswerType = answer.AnswerType,
          FeedbackType =  QuestionFeedbackType.Neutral,
          QuestionId = answer.QuestionId,
          UserId = answer.UserId
        });
      }

      foreach (var feedback in feedbackQueries)
      {
        var alreadeAnswered =
          answerEntities.FirstOrDefault(x => x.UserId == feedback.UserId && x.QuestionId == feedback.QuestionId);

        if (alreadeAnswered != null)
          alreadeAnswered.FeedbackType = feedback.QuestionFeedbackType ;
        else
          answerEntities.Add(new AnswerEntity
          {
            AnsweredDate = RandomDay(),
            AnswerType = AnswerType.NoAnswer,
            FeedbackType =  QuestionFeedbackType.Neutral,
            QuestionId = feedback.QuestionId,
            UserId = feedback.UserId
          });
      }

      return answerEntities;
    }

    public void DistributeQuestionOwners(List<User> users, List<Question> questions)
    {
      var questionOwners = new List<int>();

      for (var i = 0; i < questions.Count; i++)
      {
        var userId = users[_random.Next(0, users.Count)].UserId;
        if (questionOwners.Any(x => x == userId))
        {
          i--;
          continue;
        }
        questions[i].AuthorId = userId;
        questionOwners.Add(userId);
      }
    }

    public List<AnswerQuery> CreateAnswers(List<User> users, List<Question> questions)
    {
      var answers = new List<AnswerQuery>();
      foreach (var question in questions)
      {
        RandomizerExtension.Shuffle(users);
        RandomizerExtension.Shuffle(users);
        var answersCount = _random.Next(190, 350);
        var firstAnswers = answersCount * question.FirstOptionPercentCount / 100;
        var secondAnswers = answersCount - firstAnswers;
        for (var i = 0; i < firstAnswers; i++)
          answers.Add(new AnswerQuery
          {
            UserId = users[i].UserId,
            AnswerType = AnswerType.First,
            QuestionId = question.QuestionId,
            AnsweredDate = RandomDay(question.AddedDate, DateTime.UtcNow)
          });

        for (var i = firstAnswers; i < firstAnswers + secondAnswers; i++)
          answers.Add(new AnswerQuery
          {
            UserId = users[i].UserId,
            AnswerType = AnswerType.Second,
            QuestionId = question.QuestionId,
            AnsweredDate = RandomDay(question.AddedDate, DateTime.UtcNow)
          });
        question.AnswersCount = answersCount;
      }

      return answers;
    }

    public List<FollowingEntity> CreateFollowersEntities(List<FollowingQuery> followerQueries)
    {
      var result = new List<FollowingEntity>();
      foreach (var followerQuery in followerQueries)
      {
        result.Add(new FollowingEntity
        {
          FollowingDate = followerQuery.FollowingDate,
          FollowingId = followerQuery.FollowingId,
          UserId = followerQuery.UserId,
          VisitsCount = _random.Next(100)
        });
      }
      return result;
    }

    public List<string> ReadMainCities()
    {
      return _filesManager.ReadMainCities(Path.Combine(FilesUrl, "MainCities.txt"));
    }

    public List<CityQuery> CreateMainCities(List<string> mainCities)
    {
      var cities = new List<CityQuery>();
      foreach (var city in mainCities) cities.Add(new CityQuery { Name = city });
      return cities;
    }

    public List<TagQuery> CreateTags(List<Question> questions)
    {
      var tags = new List<TagQuery>();
      foreach (var question in questions)
        for (var i = 0; i < question.Tags.Count; i++)
          tags.Add(new TagQuery { QuestionId = question.QuestionId, Position = i, Text = question.Tags[i] });
      return tags;
    }

    public List<QuestionQuery> CreateQuestions(List<Question> questions)
    {
      var result = new List<QuestionQuery>();
      foreach (var question in questions)
        result.Add(new QuestionQuery
        {
          AddedDate = question.AddedDate,
          AudienceType = question.AudienceType,
          Condition = question.Condition,
          FirstOption = question.FirstOption,
          OriginalBackgroundUrl = question.BackgroundUrl,
          QuestionId = question.QuestionId,
          QuestionType = question.QuestionType,
          SecondOption = question.SecondOption,
          UserId = question.AuthorId
        });
      return result;
    }

    public List<QuestionFeedbackQuery> CreateFeedbacks(List<User> users, List<Question> questions)
    {
      var feedbacks = new List<QuestionFeedbackQuery>();
      foreach (var question in questions)
      {
        RandomizerExtension.Shuffle(users);
        RandomizerExtension.Shuffle(users);
        var likesCount = _random.Next(190, question.AnswersCount) + 20;
        for (var i = 0; i < likesCount; i++)
        { 
          feedbacks.Add(new QuestionFeedbackQuery
          {
            QuestionFeedbackType = QuestionFeedbackType.Like,
            UserId = users[i].UserId,
            QuestionId = question.QuestionId
          });
        }

        var dislikesCount = likesCount * _random.Next(10,25) /100;
        for (var i = 0; i < dislikesCount; i++)
        {
          feedbacks.Add(new QuestionFeedbackQuery
          {
            QuestionFeedbackType = QuestionFeedbackType.Dislike,
            UserId = users[i].UserId,
            QuestionId = question.QuestionId
          });
        }
      }
      return feedbacks;
    }

    public List<FollowingQuery> CreateFollowers(List<User> users)
    {
      var followers = new List<FollowingQuery>();

      foreach (var t in users)
      {
        var followersCount = _random.Next(15, 80);
        for (var i = 0; i < followersCount; i++)
        {
          var following = users[_random.Next(users.Count)];
          if (followers.Any(x => x.UserId == t.UserId && x.FollowingId == following.UserId))
          {
            i = i - 1;
            continue;
          }

          followers.Add(new FollowingQuery
          {
            UserId = t.UserId,
            FollowingId = following.UserId,
            FollowingDate = DateTime.UtcNow.Add(TimeSpan.FromDays(-_random.Next(250)))
          });
          followers.Add(new FollowingQuery
          {
            UserId = following.UserId,
            FollowingId = t.UserId,
            FollowingDate = DateTime.UtcNow.Add(TimeSpan.FromDays(-_random.Next(250)))
          });
        }
      }
      return followers;
    }

    private DateTime RandomDay()
    {
      var start = new DateTime(2018, 1, 1);
      var range = (DateTime.Today - start).Days;
      return start.AddDays(_random.Next(range));
    }

    private DateTime RandomDay(DateTime start, DateTime end)
    {
      var range = (end - start).Days;
      return start.AddDays(_random.Next(range));
    }

    private DateTime RandomDay(int offsetDayFromNow)
    {
      var start = new DateTime(2018, 1, 1);
      var range = offsetDayFromNow;
      return start.AddDays(_random.Next(range));
    }

    public List<Question> FromQuestionReaderToQuestion(int questionIdOffset, List<QuestionReader> questionsBag,
      List<QuestionDescriptionReader> questionsDescriptionsBag, List<UrlMatching> backgroundsMatchingBag)
    {
      var questions = new List<Question>();
      for (var i = 0; i < questionsBag.Count; i++)
      {
        var questionBag = questionsBag[i];
        var questionDescription =
          questionsDescriptionsBag.FirstOrDefault(x => x.QuestionIds.Any(y => y == questionBag.QuestionId));
        if (questionDescription == null) continue;
        var originalBackgroundUrl = backgroundsMatchingBag
          .FirstOrDefault(x => x.OldUrl == questionDescription.BackgroundUrl)?.NewUrl;
        if (originalBackgroundUrl.IsNullOrEmpty()) continue;

        var question = new Question
        {
          QuestionId = questionIdOffset + i,
          AudienceType = AudienceType.All,
          BackgroundUrl = originalBackgroundUrl,
          Condition = questionBag.Condition,
          FirstOption = questionBag.FirstOption,
          IsAnonymous = false,
          QuestionType = QuestionType.Opened,
          SecondOption = questionBag.SecondOption,
          Tags = questionBag.Tags,
          FirstOptionPercentCount = questionBag.FirstAnswersPercent,
          SecondOptionPercentCount = questionBag.SecondAnswersPercent,
          AddedDate = RandomDay(_random.Next(0, 210))
        };
        questions.Add(question);
      }

      return questions;
    }

    public List<User> FromUserReaderToUer(int userIdOffset, List<UserReader> usersBag,
      List<UrlMatching> avatarsMacthingBag)
    {
      var users = new List<User>();
      for (var i = 0; i < usersBag.Count; i++)
      {
        var userBag = usersBag[i];
        var originalAvatarUrl = avatarsMacthingBag.FirstOrDefault(x => x.OldUrl == userBag.VkOriginalAvatrUrl)?.NewUrl;
        if (originalAvatarUrl.IsNullOrEmpty()) continue;
        var email = userBag.FirstName + "_" + userBag.LastName + "@mockmail.com";
        var user = new User
        {
          UserId = userIdOffset + i,
          BirthDate = userBag.Birthday,
          Login = userBag.FirstName + " " + userBag.LastName,
          OriginalAvatarUrl = originalAvatarUrl,
          PasswordHash = email.GetMd5HashString(),
          City = userBag.City,
          Email = email,
          RoleType = RoleType.User,
          SexType = userBag.SexType,
          Description = "",
          IsBot = true,
          IsEmailConfirmed = true,
          IsPhoneNumberConfirmed = false,
          IsTwoFactorAuthenticationEnabled = false,
          PhoneNumber = null,
          RegistrationDate = RandomDay()
        };
        users.Add(user);
      }

      SwitchSex(usersBag, SexType.Man);
      SwitchSex(usersBag, SexType.Woman);

      return users;
    }

    public (List<UserQuery> users, List<UserInfoQuery> userInfos) CreateUsers(List<User> users)
    {
      var userQueries = new List<UserQuery>();
      var userInfoQueries = new List<UserInfoQuery>();

      foreach (var user in users)
      {
        var userQuery = new UserQuery
        {
          Email = user.Email,
          IsBot = user.IsBot,
          IsEmailConfirmed = user.IsEmailConfirmed,
          IsPhoneNumberConfirmed = user.IsPhoneNumberConfirmed,
          IsTwoFactorAuthenticationEnabled = user.IsTwoFactorAuthenticationEnabled,
          PasswordHash = user.PasswordHash,
          PhoneNumber = user.PhoneNumber,
          RegistrationDate = user.RegistrationDate,
          RoleType = user.RoleType,
          UserId = user.UserId
        };
        var userInfoQuery = new UserInfoQuery
        {
          UserId = user.UserId,
          BirthDate = user.BirthDate,
          City = user.City,
          Description = user.Description,
          Login = user.Login,
          OriginalAvatarUrl = user.OriginalAvatarUrl,
          SexType = user.SexType
        };
        userQueries.Add(userQuery);
        userInfoQueries.Add(userInfoQuery);
      }

      return (userQueries, userInfoQueries);
    }

    private void SwitchSex(List<UserReader> userReaders, SexType sexType)
    {
      var users = userReaders.Where(x => x.SexType == sexType).ToList();
      for (var i = 0; i < users.Count; i++)
      {
        var jName = _random.Next(i, users.Count);
        var name = users[i].FirstName;
        users[i].FirstName = users[jName].FirstName;
        users[jName].FirstName = name;

        var kSurname = _random.Next(i, users.Count);
        var surname = users[i].LastName;
        users[i].LastName = users[kSurname].LastName;
        users[kSurname].LastName = surname;
      }
    }

    public List<QuestionDescriptionReader> ReadQuestionDescription()
    {
      return _filesManager.ReadQuestionDescrition(Path.Combine(FilesUrl, "Questions.xlsx")).ToList();
    }

    public List<QuestionReader> ReadQuestions()
    {
      return _filesManager.ReadQuestions(Path.Combine(FilesUrl, "Questions.xlsx")).ToList();
    }

    public List<UserReader> ReadUsers(int offset, int count)
    {
      return _filesManager.ReadUsers(Path.Combine(FilesUrl, "PreparedVkUsers", "Users_0.txt")).Skip(offset).Take(count)
        .ToList();
    }

    public List<UrlMatching> ReadBackgroundsMatching()
    {
      return _filesManager.ReadUrlMatching(Path.Combine(FilesUrl, "BackgroundsMatching_0.txt"));
    }

    public List<UrlMatching> ReadAvatarsMatching()
    {
      return _filesManager.ReadUrlMatching(Path.Combine(FilesUrl, "AvatarsMatching_0.txt"));
    }

    public void DownloadAvatars(List<UserReader> userReaders)
    {
      for (var i = 0; i < userReaders.Count; i++)
      {
        if (i % 20 == 0) Thread.Sleep(5000);
        var userReader = userReaders[i];
        try
        {
          var avatar = _mediaService.UploadAvatar(userReader.VkOriginalAvatrUrl, AvatarType.Custom)
            .First(x => x.Size == AvatarSizeType.Original).Url;

          _filesManager.SaveUrlMatching(Path.Combine(FilesUrl, "AvatarsMatching_0.txt"),
            new UrlMatching { OldUrl = userReader.VkOriginalAvatrUrl, NewUrl = avatar });
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          Console.WriteLine(JsonConvert.SerializeObject(userReader));
        }
      }
    }

    public void DownloadBackgrounds(List<QuestionDescriptionReader> backgroundReaders)
    {
      foreach (var userReader in backgroundReaders)
        try
        {
          var background = _mediaService.UploadBackground(userReader.BackgroundUrl, BackgroundType.Standard)
            .First(x => x.Size == BackgroundSizeType.Original).Url;
          _filesManager.SaveUrlMatching(Path.Combine(FilesUrl, "BackgroundsMatching_0.txt"),
            new UrlMatching { OldUrl = userReader.BackgroundUrl, NewUrl = background });
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          Console.WriteLine(JsonConvert.SerializeObject(userReader));
        }
    }

    public void PrepareVkUsers()
    {
      _filesManager.ProcessVkUsers(VkUsersUrl, "VkUsers", Path.Combine(FilesUrl, "PreparedVkUsers"), UsersBag);
    }
  }

  public class Range
  {
    public int Offset { get; set; }
    public int Count { get; set; }
  }
}
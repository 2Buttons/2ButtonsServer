﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonLibraries;
using DataGenerator.Data;
using DataGenerator.Data.Reader.Objects;
using DataGenerator.ScriptsGenerators.DirectInsertion.Entities;

namespace DataGenerator.ScriptsGenerators.DirectInsertion
{
  public class GeneratingManager
  {
    private const string Mail = "@mockmail.com";

    private readonly BagsManager _manager = new BagsManager();
    private readonly MediaManager _mediaManager = new MediaManager();


    private readonly Random _random = new Random();

    private readonly List<City> _bagCities;
    private readonly List<Question> _bagQuestions;
    private readonly List<EmailBlank> _bagEmails;
    private  List<User> _bagUsers;
    private List<CityEntity> _cities;
    private List<OptionEntity> _options;
    private List<QuestionEntity> _questions;
    private List<UserInfoEntity> _userInfos;
    private List<UserEntity> _users;
    private List<AnswerEntity> _answers;
    private List<FollowEntity> _follows;

    public void DownloadPhotos()
    {
      var users = _manager.LoadUsers(1000, 0, 0).ToList();
      var userEntities =
        users.Select(x => new UserInfoEntity
        {
          UserId = x.UserId,
          OriginalAvatarUrl = x.LargePhoto,
          //SmallAvatarLink = x.SmallPhoto
        }).ToList();
      _mediaManager.SetAvatars(userEntities, @"E:\Projects\2Buttons\Project\Data\Media\");

    }

    public GeneratingManager(Range users, Range questions)
    {

      _bagCities = _manager.LoadCities().ToList();
      _bagQuestions = _manager.LoadQuestions().Skip(questions.Offset).Take(questions.Count).ToList();
      _bagUsers = _manager.LoadUsers(users.Count, users.Offset, 0).ToList();
      _bagEmails = _manager.LoadEmails();
    }

    public void PreprocessData()
    {
      ToEntitys(_bagCities);
      ToEntitys(_bagQuestions,2000);
      ToEntitys(_bagUsers);
      _answers = new List<AnswerEntity>();
      _follows = new List<FollowEntity>();

      CalculatePopulationInCities();
      DistributeQuestions();
      DistributeAnswers();
      DistributeFollowers();

    }

    public void CreateScripts()
    {
      var path = @"E:\Projects\2Buttons\Project\Data\Scripts\";
      var optionsUrl = "Options.sql";
      var questionsUrl = "Questions.sql";
      var usersUrl = "Users.sql";
      var usersInfoUrl = "UsersInfo.sql";
      var citiesUrl = "Cities.sql";
      var answersUrl = "Answers.sql";
      var followsUrl = "Follows.sql";
      using (var sw = new StreamWriter(Path.Combine(path, optionsUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new OptionGenerator().GetInsertionLine(_options));
      }

      using (var sw = new StreamWriter(Path.Combine(path, questionsUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new QuestionGenerator().GetInsertionLine(_questions));
      }

      using (var sw = new StreamWriter(Path.Combine(path, usersUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new UserGenerator().GetInsertionLineAccount(_users));
      }

      using (var sw = new StreamWriter(Path.Combine(path, usersInfoUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new UserGenerator().GetInsertionLineMain(_userInfos));
      }

      using (var sw = new StreamWriter(Path.Combine(path, citiesUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new CityGenerator().GetInsertionLine(_cities));
      }

      using (var sw = new StreamWriter(Path.Combine(path, answersUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new AnswerGenerator().GetInsertionLine(_answers));
      }

      using (var sw = new StreamWriter(Path.Combine(path, followsUrl), false, Encoding.UTF8))
      {
        sw.WriteLine(new FollowGenerator().GetInsertionLine(_follows));
      }

    }

    private void ToEntitys(IEnumerable<City> cities)
    {
      _cities = cities.Select(x => new CityEntity { CityId = x.CityId, Name = x.Title, Inhabitants = 0 }).ToList();
    }

    private void ToEntitys(IList<Question> questions, int startIndex)
    {
      _options = new List<OptionEntity>();
      _questions = new List<QuestionEntity>();

      for (int i = 0; i < questions.Count; i++)
      {
        questions[i].QuestionId = startIndex + i;
      }

      foreach (var t in questions)
      {
        _options.Add(new OptionEntity
        {
          OptionId = t.QuestionId * 2 - 1,
          QuestionId = t.QuestionId,
          Text = t.FirstOption,
          Position = 1,
          AnswersCount = 0,
        });

        _options.Add(new OptionEntity
        {
          OptionId = t.QuestionId * 2,
          QuestionId = t.QuestionId,
          Text = t.SecondOption,
          Position = 2,
          AnswersCount = 0,
        });
        _questions.Add(new QuestionEntity
        {
          QuestionId = t.QuestionId,
          AudienceType = AudienceType.All,
          OriginalBackgroundUrl = "",
          Condition = t.Condition,
          DislikesCount = 0,
          IsDeleted = false,
          IsAnonimoty = false,
          LikesCount = 0,
          QuestionAddDate = RandomDay(),
          QuestionType = QuestionType.Opened,
          //Shows = 0,
          UserId = 0
        });
        _mediaManager.SetStandardsBackground(_questions, @"E:\Projects\2Buttons\Project\MediaData\standards\", @"E:\Projects\2Buttons\Project\Data\Media\");
      }


    }
    private void ToEntitys(IList<User> users)
    {
      _users = new List<UserEntity>();
      _userInfos = new List<UserInfoEntity>();

      foreach (User t in users)
      {
        var email = _bagEmails[_random.Next(_bagEmails.Count)].Text + $"{_random.Next(1000)}"+$"{t.UserId}" + Mail;
        var user = new UserEntity
        {
          UserId = t.UserId,
          RoleType = RoleType.User,
          RegistrationDate = RandomDay(),
          Email = email,
          PasswordHash = email.GetMd5Hash(),
          PhoneNumber = null
        };
        _users.Add(user);
        var userInfo = new UserInfoEntity
        {
          UserId = t.UserId,
          BirthDate = t.Birthday,
          CityId = t.City.CityId,
          Description = null,
          Login = t.FirstName + " " + t.LastName,
          SexType = t.Sex,
          LastNotsSeenDate = DateTime.Now,
          OriginalAvatarUrl = t.LargePhoto,
          //SmallAvatarLink = t.SmallPhoto
        };
        _userInfos.Add(userInfo);
      }
      _mediaManager.SetAvatars(_userInfos, @"E:\Projects\2Buttons\Project\Data\Media\");

    }

    private void DistributeQuestions()
    {


      var restQuestions = _questions.ToList();


      foreach (var t in _users)
      {
        if (restQuestions.Count <= 0) return;
        var questionIndex = _random.Next(0, restQuestions.Count);
        restQuestions[questionIndex].UserId = t.UserId;
        restQuestions.RemoveAt(questionIndex);
      }
    }

    private void CalculatePopulationInCities()
    {
      foreach (var city in _cities)
      {
        city.Inhabitants = _userInfos.Count(x => x.CityId == city.CityId);
      }
    }

    private void DistributeAnswers()
    {
      foreach (var t in _users)
      {
        for (int i = 0; i < 35; i++)
        {
          var questionIndex = _random.Next(0, _questions.Count);
          var answer = _random.Next(1, 3);
          if (_answers.Any(x => x.QuestionId == _questions[questionIndex].QuestionId && x.UserId == t.UserId)) continue;
          _answers.Add(new AnswerEntity
          {
            UserId = t.UserId,
            QuestionId = _questions[questionIndex].QuestionId,
            AnswerDate = DateTime.UtcNow.Add(TimeSpan.FromDays(-_random.Next(250))),
            AnswerType = (AnswerType) answer,
            IsLiked = false
          });
          _options.First(x => x.QuestionId == _questions[questionIndex].QuestionId && x.Position == answer).AnswersCount++;
         // _questions[questionIndex].Shows++;
          if (_random.Next(100) % 5 == 0) _questions[questionIndex].LikesCount++;
        }
      }
    }

    private void DistributeFollowers()
    {
      foreach (var t in _users)
      {
        var followersCount = _random.Next(15, 80);
        for (int i = 0; i < followersCount; i++)
        {
          var following = _users[_random.Next(_users.Count)];
          if (_follows.Any(x => x.UserId == t.UserId && x.FollowingId == following.UserId))
          {
            i = i - 1;
            continue;
          }

          _follows.Add(new FollowEntity
          {
            UserId = t.UserId,
            FollowingId = following.UserId,
            FollowedDate = DateTime.UtcNow.Add(TimeSpan.FromDays(-_random.Next(250))),
            VisitsCount = _random.Next(100)
          });
          _follows.Add(new FollowEntity
          {
            UserId = following.UserId,
            FollowingId = t.UserId,
            FollowedDate = DateTime.UtcNow.Add(TimeSpan.FromDays(-_random.Next(250))),
            VisitsCount = _random.Next(100)
          });

        }
      }
    }

    private DateTime RandomDay()
    {
      DateTime start = new DateTime(2018, 1, 1);
      int range = (DateTime.Today - start).Days;
      return start.AddDays(_random.Next(range));
    }
  }

  public class Range
  {
    public int Offset { get; set; }
    public int Count { get; set; }
  }
}
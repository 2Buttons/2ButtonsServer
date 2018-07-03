using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonLibraries;
using DataGenerator.Data;
using DataGenerator.Data.Reader.Objects;
using DataGenerator.Data.ReaderObjects;
using DataGenerator.ScriptsGenerators.Entities;
using Microsoft.Extensions.Options;

namespace DataGenerator.ScriptsGenerators
{
  public class GeneratingManager
  {
    private BagsManager _manager = new BagsManager();
    private MediaManager _mediaManager = new MediaManager();


    private Random _random = new Random();

    private List<City> _bagCities;
    private List<Question> _bagQuestions;
    private List<EmailBlank> _bagEmails;
    private List<User> _bagUsers;
    private List<CityEntity> _cities;
    private List<OptionEntity> _options;
    private List<QuestionEntity> _questions;
    private List<UserInfoEntity> _userInfos;
    private List<UserEntity> _users;



    public GeneratingManager(Range users, Range questions)
    {
      _bagCities = _manager.LoadCities().ToList();
      _bagQuestions = _manager.LoadQuestions().Skip(questions.From).Take(questions.To).ToList();
      _bagUsers = _manager.LoadUsers().Skip(users.From).Take(users.To).ToList();
      _bagEmails = _manager.LoadEmails();
      PrepareEmails();
      ToEntitys(_bagCities);
      ToEntitys(_bagQuestions);
      ToEntitys(_bagUsers);
    }


    public void CreateScripts()
    {
      var path = @"E:\Projects\2Buttons\Project\Data\Scripts\";
      var optionsUrl = "Options.sql";
      var questionsUrl = "Questions.sql";
      var usersUrl = "Users.sql";
      var usersInfoUrl = "UsersInfo.sql";
      var citiesUrl = "Cities.sql";
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

    }

    private void PrepareEmails()
    {
      const string mail = "@mockmail.com";
      for (var i = 0; i < _bagEmails.Count; i++)
      {
        //var t = _bagEmails[i];
        _bagEmails[i].Text = _bagEmails[i].Text + $"{_bagEmails[i].Text}" + $"{i}" + mail;
      }
    }

    private void ToEntitys(IEnumerable<City> cities)
    {
      _cities = cities.Select(x => new CityEntity { CityId = x.CityId, Title = x.Title, People = 0 }).ToList();
    }

    private void ToEntitys(IEnumerable<Question> questions)
    {
      _options = new List<OptionEntity>();
      _questions = new List<QuestionEntity>();

      foreach (var t in questions)
      {
        _options.Add(new OptionEntity
        {
          OptionId = t.QuestionId * 2 - 1,
          QuestionId = t.QuestionId,
          OptionText = t.FirstOption,
          Position = 1,
          Answers = 0,
        });

        _options.Add(new OptionEntity
        {
          OptionId = t.QuestionId * 2,
          QuestionId = t.QuestionId,
          OptionText = t.SecondOption,
          Position = 2,
          Answers = 0,
        });
        _questions.Add(new QuestionEntity
        {
          QuestionId = t.QuestionId,
          AudienceType = AudienceType.All,
          BackgroundImageLink = "",
          Condition = t.Condition,
          Dislikes = 0,
          IsDeleted = false,
          IsAnonimoty = false,
          Likes = 0,
          QuestionAddDate = RandomDay(),
          QuestionType = QuestionType.Opened,
          Shows = 0,
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
        var email = _bagEmails[_random.Next(_bagEmails.Count)].Text;
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
          CityId = t.CityId,
          Description = null,
          Login = t.FirstName + " " + t.LastName,
          SexType = t.Sex,
          LastNotsSeenDate = DateTime.Now,
          LargeAvatarLink = t.LargePhoto,
          SmallAvatarLink = t.SmallPhoto
        };
        _userInfos.Add(userInfo);
      }
      _mediaManager.SetAvatars(_userInfos, @"E:\Projects\2Buttons\Project\Data\Media\");

    }

    public void DistributeQuestions()
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

    public void CalculatePopulationInCities()
    {
      foreach (var city in _cities)
      {
        city.People = _userInfos.Count(x => x.CityId == city.CityId);
      }
    }

    //public void CreateAnswers()
    //{
    //  foreach (var t in _users)
    //  {
    //    for (int i = 0; i < 25; i++)
    //    {
    //      var questionIndex = _random.Next(0, _questions.Count);
    //      var answer = _random.Next(1, 3);
    //      if(_)
    //    }
    //  }
    //}

    private DateTime RandomDay()
    {
      DateTime start = new DateTime(2018, 1, 1);
      int range = (DateTime.Today - start).Days;
      return start.AddDays(_random.Next(range));
    }
  }

  public class Range
  {
    public int From { get; set; }
    public int To { get; set; }
  }
}
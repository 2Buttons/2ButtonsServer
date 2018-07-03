using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonLibraries.Extensions;
using DataGenerator.Data.Reader;
using Newtonsoft.Json;

namespace DataGenerator.Data
{
  public class BagsManager
  {
    public const string QuestionsFile = @"Questions.xlsx";
    public const string UsersFile = "Users.txt";
    public const string MainCitiesFile = "MainCities.txt";

    public const string UsersBag = "Users.txt";
    public const string QuestionsBag = "Questions.txt";
    public const string CitiesBag = "Cities.txt";
    public const string CitiesMatching = "CitiesMatching.txt";

    public const string VkUsersUrl = @"E:\Projects\2Buttons\Project\VkUsers\";

    public const string BagsUrl = @"E:\Projects\2Buttons\Project\Data\Bags\";

    public const string FilesUrl = @"E:\Projects\2Buttons\Project\Data\Files\";

    public const string ScriptsUrl = @"E:\Projects\2Buttons\Project\Data\Scripts\";

    private readonly List<CityMatching> _citiesMatching = new List<CityMatching>();
    private readonly List<int> _questionsIds = new List<int>();

    private readonly Random _random = new Random();

    private readonly FilesReader _reader = new FilesReader();



    public void SaveBags()
    {
      SaveCitiesBag();
      SaveCitiesMatchingBag();
      SaveUsersBag();
      SaveQuestionsBag();
    }

    private void SaveCitiesBag()
    {
      var mainCities = _reader.ReadMainCities(FilesUrl + MainCitiesFile);
      var usersCitis = _reader.ReadCities(FilesUrl + UsersFile);

      var cities = new List<City>();
      cities.AddRange(mainCities);
      cities.AddRange(usersCitis);

      cities = cities.OrderBy(x => x.CityId).Distinct(new City()).ToList();

      for (var i = 0; i < cities.Count; i++)
      {
        _citiesMatching.Add(new CityMatching
        {
          VkId = cities[i].CityId,
          TwoBId = cities[i].CityId,
          Title = cities[i].Title
        });
        cities[i].CityId = i + 1;
      }

      using (var sw = new StreamWriter(BagsUrl + CitiesBag, false, Encoding.UTF8))
      {
        sw.WriteLine(JsonConvert.SerializeObject(cities));
      }
    }

    private void SaveCitiesMatchingBag()
    {
      using (var sw = new StreamWriter(BagsUrl + CitiesMatching, false, Encoding.UTF8))
      {
        sw.WriteLine(JsonConvert.SerializeObject(_citiesMatching));
      }
    }

    private void SaveQuestionsBag()
    {
      var questions = _reader.ReadQuestions(FilesUrl + QuestionsFile);

      using (var sw = new StreamWriter(BagsUrl + QuestionsBag, false, Encoding.UTF8))
      {
        sw.WriteLine(JsonConvert.SerializeObject(questions));
      }
    }

    private void SaveUsersBag()
    {
      var users = _reader.ReadUsers(FilesUrl + UsersFile).OrderBy(x => x.Birthday.Age()).ToList();

      for (var i = 0; i < users.Count; i++)
      {
        users[i].CityId = _citiesMatching.First(x => x.VkId == users[i].CityId).TwoBId;
        users[i].UserId = i + 1;

        var jName = _random.Next(i, users.Count);
        var name = users[i].FirstName;
        users[i].FirstName = users[jName].FirstName;
        users[jName].FirstName = name;

        var kSurname = _random.Next(i, users.Count);
        var surname = users[i].LastName;
        users[i].LastName = users[kSurname].LastName;
        users[kSurname].LastName = surname;
      }

      using (var sw = new StreamWriter(BagsUrl + UsersBag, false, Encoding.UTF8))
      {
        sw.WriteLine(JsonConvert.SerializeObject(users));
      }
    }
  }

  public class CityMatching
  {
    public int VkId { get; set; }
    public int TwoBId { get; set; }
    public string Title { get; set; }
  }
}
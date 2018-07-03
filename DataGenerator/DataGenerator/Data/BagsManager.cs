using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonLibraries.Extensions;
using DataGenerator.Data.Reader;
using DataGenerator.Data.Reader.Objects;
using DataGenerator.Data.ReaderObjects;
using Newtonsoft.Json;

namespace DataGenerator.Data
{
  public class BagsManager
  {
    public const string QuestionsFile = @"Questions.xlsx";
    public const string UsersFile = "Users.txt";
    public const string MainCitiesFile = "MainCities.txt";
    public const string EmailFile = "EnglishWord.txt";

    public const string UsersBag = "Users.txt";
    public const string QuestionsBag = "Questions.txt";
    public const string EmailsBag = "Emails.txt";
    public const string CitiesBag = "Cities.txt";
    public const string CitiesMatching = "CitiesMatching.txt";

    public const string VkUsersUrl = @"E:\Projects\2Buttons\Project\Data\Files\VkUsers\";

    public const string BagsUrl = @"E:\Projects\2Buttons\Project\Data\Bags\";

    public const string FilesUrl = @"E:\Projects\2Buttons\Project\Data\Files\";

    public const string ScriptsUrl = @"E:\Projects\2Buttons\Project\Data\Scripts\";

    private readonly List<CityMatching> _citiesMatching = new List<CityMatching>();
    private readonly List<int> _questionsIds = new List<int>();

    private readonly Random _random = new Random();

    private readonly FilesReader _reader = new FilesReader();
    private readonly BagsReader _bagReader = new BagsReader();

    public List<City> LoadCities()
    {
      return _bagReader.ReadCities(BagsUrl + CitiesBag);
    }

    public List<User> LoadUsers()
    {

      return _bagReader.ReadUsers(BagsUrl + UsersBag);
    }

    public List<EmailBlank> LoadEmails()
    {

      return _bagReader.ReadEmails(BagsUrl + EmailsBag);
    }

    public List<Question> LoadQuestions()
    {

      return _bagReader.ReadQuestions(BagsUrl + QuestionsBag);
    }

    public void SaveBags()
    {
      SaveCitiesBag();
      SaveCitiesMatchingBag();
      SaveUsersBag();
      SaveQuestionsBag();
      SaveEmailBags();
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
          TwoBId = i+1,
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
      }
      SwitchFemaleData(users);
      SwitchMaleData(users);

      using (var sw = new StreamWriter(BagsUrl + UsersBag, false, Encoding.UTF8))
      {
        sw.WriteLine(JsonConvert.SerializeObject(users));
      }
    }

    private void SaveEmailBags()
    {
     
      var emailsStrings = _reader.ReadEnglishWords(FilesUrl + EmailFile);
      var result = new List<EmailBlank>();
      for (var i = 0; i < emailsStrings.Count; i++)
      {
        var email = new EmailBlank { Text = emailsStrings[i]};
        result.Add(email);
      }
      using (var sw = new StreamWriter(BagsUrl + EmailsBag, false, Encoding.UTF8))
      {
        sw.WriteLine(JsonConvert.SerializeObject(result));
      }
    }

    private void SwitchMaleData(List<User> users)
    {

      users = users.Where(x => x.Sex == CommonLibraries.SexType.Man).ToList();
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

    private void SwitchFemaleData(List<User> users)
    {

      users = users.Where(x => x.Sex == CommonLibraries.SexType.Woman).ToList();
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

    public void CombineUsers()
    {
      _reader.CombineVkUsers(VkUsersUrl, "VkUsers", FilesUrl, UsersBag);
    }
  }

  public class CityMatching
  {
    public int VkId { get; set; }
    public int TwoBId { get; set; }
    public string Title { get; set; }
  }
}
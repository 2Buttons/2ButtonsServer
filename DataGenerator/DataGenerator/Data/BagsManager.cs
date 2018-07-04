using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using DataGenerator.Data.Reader;
using DataGenerator.Data.Reader.Objects;
using Newtonsoft.Json;

namespace DataGenerator.Data
{
  public class BagsManager
  {

    private class CitiyCount
    {
      public City City { get; set; }
      public List<int> VkkIds { get; set; }
    }

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

    public List<User> LoadUsers(int count, int offset, int fileIndex)
    {
      if (count > 100_000) throw new Exception("Count has to equal or less than 100 000");
      var file = Directory.GetFiles(BagsUrl).Where(x => x.Contains("Users")).ToList()[fileIndex];
      return _bagReader.ReadUsers(file).Skip(offset).Take(count).ToList();

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
      
      var files = Directory.GetFiles(FilesUrl).Where(x => x.Contains("Users")).ToList();
      var citiesArray = new List<List<City>>();
      foreach (var file in files)
      {
        citiesArray.Add(new List<City>());
      }

      Parallel.For(0, files.Count, (i) =>
      
      {
        citiesArray[i] = _reader.ReadCities(files[i]).Select(x => new City { CityId = x.CityId, Title = x.Title.Replace("'", "''") })
          .Distinct(new City()).ToList();
      });

      var userCities = new List<City>();

      userCities.AddRange(mainCities);
      foreach (var items in citiesArray)
      {
        userCities.AddRange(items);
        userCities = userCities.Distinct(new City()).ToList();
      }
   

      var cities = new List<CitiyCount>();


     // var predCities = userCities;
      foreach (var city in userCities)
      {
        var existingCity = cities.FirstOrDefault(x => x.City.Title == city.Title);
        if (existingCity != null && existingCity.VkkIds.All(x => x != city.CityId)) existingCity.VkkIds.Add(city.CityId);
        else cities.Add(new CitiyCount {City = city, VkkIds = new List<int>()});
      }

      var london = cities.FirstOrDefault(x => x.City.Title == "London");
      var ahelezn = cities.First(x => x.City.Title == "Железногорск").VkkIds.Count();
      for (var i = 0; i < cities.Count; i++)
      {
        _citiesMatching.Add(new CityMatching
        {
          VkIds = cities[i].VkkIds,
          TwoBId = i + 1,
          Title = cities[i].City.Title
        });
        cities[i].City.CityId = i + 1;
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
      var offset = 100;
      var files = Directory.GetFiles(FilesUrl).Where(x => x.Contains("Users")).ToList();
      for (var i = 0; i < files.Count; i++)
      {
        var file = files[i];
        var users = _reader.ReadUsers(file)
          .Select(x => new User
          {
            UserId = x.UserId,
            FirstName = x.FirstName.Replace("'", "''"),
            LastName = x.LastName.Replace("'", "''"),
            Sex = x.Sex,
            Birthday = x.Birthday,
            CityId = x.City.CityId,
            SmallPhoto = x.SmallPhoto,
            LargePhoto = x.LargePhoto
          }).ToList().OrderBy(x => x.Birthday.Age()).ToList();

        for (var k = 0; k < users.Count; k++)
        {
          users[k].CityId = _citiesMatching.First(x => x.VkIds.Any(y=>y== users[k].CityId)).TwoBId;
          users[k].UserId = k + offset;
        }

        offset += users.Count;
        SwitchFemaleData(users);
        SwitchMaleData(users);

        using (var sw =
          new StreamWriter(BagsUrl + Path.GetFileNameWithoutExtension(UsersBag) + $"_{i}" + Path.GetExtension(UsersBag),
            false, Encoding.UTF8))
        {
          sw.WriteLine(JsonConvert.SerializeObject(users));
        }
      }
    }

    private void SaveEmailBags()
    {

      var emailsStrings = _reader.ReadEnglishWords(FilesUrl + EmailFile);
      var result = new List<EmailBlank>();
      for (var i = 0; i < emailsStrings.Count; i++)
      {
        var email = new EmailBlank { Text = emailsStrings[i] };
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
      _reader.ProcessVkUsers(VkUsersUrl, "VkUsers", FilesUrl, UsersBag);
    }
  }

  public class CityMatching
  {
    public List<int> VkIds { get; set; }
    public int TwoBId { get; set; }
    public string Title { get; set; }
  }
}
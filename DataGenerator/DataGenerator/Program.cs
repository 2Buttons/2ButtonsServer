using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonLibraries.Extensions;
using DataGenerator.Data;
using DataGenerator.Data.Reader.Objects;
using DataGenerator.ScriptsGenerators;
using DataGenerator.ScriptsGenerators.Entities;
using Newtonsoft.Json;

namespace DataGenerator
{
  internal class Program
  {
    public const string CitiesUrl = @"E:\Projects\2Buttons\Project\Data\Bags\Cities.txt";
    public const string CitiesMatchingUrl = @"E:\Projects\2Buttons\Project\Data\Bags\CitiesMatching.txt";

    public const string QuestionsUrl =
      @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\Questions.xlsx";

    public const string MaleNamesUrl =
      @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\MaleNames.txt";

    public const string FemaleNamesUrl =
      @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\FemaleNames.txt";

    public const string SurnamesUrl =
      @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\Surnames.txt";

    public const string VkUsersUrl =
      @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\VkUsers";

    public const string FolderUrl =
      @"E:\Projects\2Buttons\Project\Data\Bags\";


    private static void Main(string[] args)
    {
       //GeneratingManager manager = new GeneratingManager(new Range { Offset = 0, Count = 1000 }, new Range { Offset = 0, Count = 150 });
      //manager.DownloadPhotos();
      //Console.ReadLine();

      // manager.PreprocessData();
      //manager.CreateScripts();
      //BagsManager bags = new BagsManager();
      //bags.CombineUsers();
      //bags.SaveBags();


      //using (var sr = new StreamReader(@"E:\Projects\2Buttons\Project\Data\Scripts\Users.sql", Encoding.UTF8))
     // {

        var usersStat = new List<StatisticsEntity>();
      for (int i = 100; i <= 1099; i++)
      {
        usersStat.Add(new StatisticsEntity {UserId = i});
      }
        var script = new StatisticsGenerator().GetInsertionLine(usersStat);

        using (var sw = new StreamWriter(@"E:\Projects\2Buttons\Project\Data\Scripts\Statistics.sql", false, Encoding.UTF8))
        {
          sw.WriteLine(script);
        }
     // }

      //List<City> cities;

      //using (var sw = new StreamReader(CitiesUrl, Encoding.UTF8))
      //{
      //  cities = JsonConvert.DeserializeObject<List<City>>(sw.ReadToEnd());
      //}

      //for (int i = 0; i < cities.Count; i++)
      //{
      //  cities[i].CityId = cities[i].CityId + 2000;
      //}

      //using (var sw = new StreamWriter(CitiesUrl, false, Encoding.UTF8))
      //{
      //  sw.WriteLine(JsonConvert.SerializeObject(cities));
      //}

      //cities.Clear();


      //List<CityMatching> citiesMacthin;

      //using (var sw = new StreamReader(CitiesMatchingUrl, Encoding.UTF8))
      //{
      //  citiesMacthin = JsonConvert.DeserializeObject<List<CityMatching>>(sw.ReadToEnd());
      //}

      //for (int i = 0; i < citiesMacthin.Count; i++)
      //{
      //  citiesMacthin[i].TwoBId = citiesMacthin[i].TwoBId + 2000;
      //}

      //using (var sw = new StreamWriter(CitiesMatchingUrl, false, Encoding.UTF8))
      //{
      //  sw.WriteLine(JsonConvert.SerializeObject(citiesMacthin));
      //}

      //citiesMacthin.Clear();

      //List<User> users;

      //var files = Directory.GetFiles(FolderUrl).Where(x => x.Contains("Users")).ToList();

      //foreach (var file in files)
      //{
      //  using (var sw = new StreamReader(file, Encoding.UTF8))
      //  {
      //    users = JsonConvert.DeserializeObject<List<User>>(sw.ReadToEnd());
      //  }

      //  for (int i = 0; i < citiesMacthin.Count; i++)
      //  {
      //    users[i].City.CityId = users[i].City.CityId + 2000;
      //  }

      //  using (var sw = new StreamWriter(file, false, Encoding.UTF8))
      //  {
      //    sw.WriteLine(JsonConvert.SerializeObject(users));
      //  }

      //}





      //manager.CalculatePopulationInCities();
      //manager.DistributeQuestions();
      //manager.CreateScripts();

      //var users = bags.LoadUsers();
      //var av = users.Where(x => x.Birthday.Age() >= 14 && x.Birthday.Age() <= 25).Average(x => x.Birthday.Age());
      //var t = av;

      //var reader = new Reader();


      //var vkUsers = reader.ReadUserFromVkFile(VkUsersUrl + "_opros_tyta_0_.txt");

      //var cities = vkUsers.Where(x=>x.City!=null).Select(x => new CityEntity {CityId = x.City.CityId, Name = x.City.Title, People = 0}).ToList();

      //var citySQL = new CityGenerator().GetFullInsertionLine(cities);

      //using (StreamWriter sw = new StreamWriter(FolderUrl+"CitiesFromVK.txt", false, System.Text.Encoding.UTF8))
      //{
      //  sw.WriteLine(citySQL);
      //}

      //if (File.Exists(@"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\Questions.xlsx"))
      //{
      //  var  p  = reader.ReadQuestions(@"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\Questions.xlsx");

      //  foreach (var t in p)
      //  {
      //    Console.WriteLine($"{t.QuestionId} {t.Condition} {t.FirstOption} {t.SecondOption}");
      //  }
      //}

      // var p = reader.ReadSurnames(SurnamesUrl);

      //foreach (var t in p) Console.WriteLine($"{t.Title} {t.Gender}");




      // VkService service = new VkService();

      //var users = service.GetUsersFromGroup(1,AppAccess).GetAwaiter().GetResult();


      //using (StreamWriter sw = new StreamWriter(VkUsersUrl, false, System.Text.Encoding.UTF8))
      //{
      //  foreach (var user in users)
      //  {
      //    sw.WriteLine(Json.Ser)
      //  }
      //}




      //var groupId = "opros_tyta";

      //for (int i = 0; i < 100; i++)
      //{
      //  var path = VkUsersUrl + "_" + groupId + $"_{i}_" + ".txt";
      //  var users = service.GetJsonFromGroup(groupId, i, AppAccess).GetAwaiter().GetResult();
      //  using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.UTF8))
      //  {

      //    sw.WriteLine(users);

      //  }
      //  Thread.Sleep(4000);
      //}

      //var t = new Reader().ReadUserFromVkFile(VkUsersUrl + "_opros_tyta_0_.txt");
      //var m = t;
      //Console.ReadKey();

      //new VkService().WriteMemberGroupsStringToFile("mudakoff", 10_000_000,
      // "8ca841528ca841528ca84152578ccaf9b288ca88ca84152d645d207a5fae99916d1944a", FolderUrl).GetAwaiter().GetResult();
      //new UsersManaging().GetUsersFromVkFiles(FolderUrl);

      //var users = new Reader().ReadUsers(FolderUrl + "Users.txt");
      //var count = users.Count(x => x.Birthday.Age() > 16 && x.Birthday.Age() < 21);
      //var l = count;
      //var m = users.Where(x => x.Birthday.Age() >= 14 && x.Birthday.Age() <= 25).ToList();
      //var average = m.Average(x => x.Birthday.Age());
      //var p = m;
    }

    //public async Task<string> GetJsonFromGroup(string groupId, int offset, string externalToken)
    //{
    //  offset = offset * 1000;
    //  return await new HttpClient().GetStringAsync(
    //    $"https://api.vk.com/method/groups.getMembers?group_id={groupId}&offset={offset}&lang=0&fields=first_name,last_name,sex,bdate,city,photo_100,photo_max_orig&access_token={externalToken}&v=5.80");

    //}

  }
}
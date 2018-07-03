using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonLibraries.Extensions;
using DataGenerator.Data.ReaderObjects;
using DataGenerator.VkCrawler;
using Independentsoft.Office.Spreadsheet;
using Newtonsoft.Json;

namespace DataGenerator.Data.Reader
{
  public class FilesReader
  {
    //public const string QuestionsUrl =
    //  @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\Questions.xlsx";

    //public const string VkUsersUrl = @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\VkUsers";

    //public const string FolderUrl = @"E:\Projects\2Buttons\Project\VkUsers\";

    public List<City> ReadMainCities(string path)
    {
      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        var json = sr.ReadToEnd();
        return JsonConvert.DeserializeObject<VkCityResponse>(json).Response.Items
          .Select(x => new City {CityId = x.CityId, Title = x.Title}).ToList();
      }
    }

    public List<string> ReadEnglishWords(string path)
    {
      var result = new List<string>();
      using (var sr = new StreamReader(path, Encoding.UTF8))
      {

        string line;
        while ((line = sr.ReadLine()) != null)
        {
          if (line != string.Empty)
          {
            line = line.Trim().Split(' ').ElementAt(2);
            result.Add(line);
          }
        }
        //var line = sr.ReadLine();
        //var lines = line.Trim().Split(' ').ToList();
        
      }

      return result;
    }

    public List<Question> ReadQuestions(string path)
    {
      var result = new List<Question>();
      var book = new Workbook(path);

      foreach (var sheet in book.Sheets)
        if (sheet is Worksheet worksheet)
        {
          var cells = worksheet.GetCells();

          for (var i = 0; i < cells.Count; i++)
          {
            var question = new Question
            {
              QuestionId = int.Parse(cells[i].Value),
              Condition = cells[i + 1].Value.Replace("'", "''"),
              FirstOption = cells[i + 2].Value.Replace("'", "''"),
              SecondOption = cells[i + 5].Value.Replace("'", "''")
            };
            result.Add(question);
            i = i + 5;
          }
        }
      return result;
    }

    public List<City> ReadCities(string path)
    {
      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        return JsonConvert.DeserializeObject<List<VkUserData>>(sr.ReadToEnd()).Where(x=>x.City !=null).OrderBy(x=>x.City.CityId)
          .Select(x =>  new City {CityId = x.City.CityId, Title = x?.City.Title.Replace("'", "''") }).ToList();
      }
    }

    public List<User> ReadUsers(string path)
    {
      using (var sr = new StreamReader(path))
      {
        return JsonConvert.DeserializeObject<List<VkUserData>>(sr.ReadToEnd()).Where(x=>x.Birthday.Age()>10 && !x.LargePhoto.Contains("deactivated") &&x.City !=null).Select(x => new User
        {
          UserId = x.UserId,
          FirstName = x.FirstName.Replace("'", "''"),
          LastName = x.LastName.Replace("'", "''"),
          Sex = x.Sex,
          Birthday = x.Birthday,
          CityId = x.City.CityId,
          SmallPhoto = x.SmallPhoto,
          LargePhoto = x.LargePhoto
        }).ToList();
      }
    }

    public void CombineVkUsers(string folderFromPath, string pattern, string folderPathTo, string fileName)
    {
      var result = new List<VkUserData>();

      var files = Directory.GetFiles(folderFromPath).Where(x => x.Contains(pattern)).ToList();
      foreach (var file in files)
        using (var sr = new StreamReader(file))
        {
          var json = sr.ReadToEnd().Replace("29.2", "25.2").Replace("32.5", "30.5").Replace("No","").Replace("Name","");
          var jsonUsers = JsonConvert.DeserializeObject<VkUserDataResponse>(json).Response.Items.Where(x=>x.Birthday.Age()>5).ToList();
          result.AddRange(jsonUsers);
        }

      using (var sw = new StreamWriter(folderPathTo + fileName, false, Encoding.UTF8))
      {
        sw.WriteLine(JsonConvert.SerializeObject(result));
      }
    }
  }
}
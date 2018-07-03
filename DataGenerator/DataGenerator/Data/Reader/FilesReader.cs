using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataGenerator.Data.ReaderObjects;
using DataGenerator.VkCrawler;
using Independentsoft.Office.Spreadsheet;
using Newtonsoft.Json;

namespace DataGenerator.Data.Reader
{
  public class FilesReader
  {
    public const string QuestionsUrl =
      @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\Questions.xlsx";

    public const string VkUsersUrl = @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\VkUsers";

    public const string FolderUrl = @"E:\Projects\2Buttons\Project\VkUsers\";

    public List<City> ReadMainCities(string path)
    {
      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        return JsonConvert.DeserializeObject<VkCityResponse>(sr.ReadToEnd()).Response
          .Select(x => new City {CityId = x.CityId, Title = x.Title}).ToList();
      }
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
              Condition = cells[i + 1].Value,
              FirstOption = cells[i + 2].Value,
              SecondOption = cells[i + 5].Value
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
        return JsonConvert.DeserializeObject<List<VkUserData>>(sr.ReadToEnd())
          .Select(x => new City {CityId = x.City.CityId, Title = x.City.Title}).ToList();
      }
    }

    public List<User> ReadUsers(string path)
    {
      using (var sr = new StreamReader(path))
      {
        return JsonConvert.DeserializeObject<List<VkUserData>>(sr.ReadToEnd()).Select(x => new User
        {
          UserId = x.UserId,
          FirstName = x.FirstName,
          LastName = x.LastName,
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
          var json = sr.ReadToEnd().Replace("29.2", "25.2").Replace("32.5", "30.5");
          var jsonUsers = JsonConvert.DeserializeObject<VkUserDataResponse>(json).Response.Items.ToList();
          result.AddRange(jsonUsers);
        }

      using (var sw = new StreamWriter(folderPathTo + fileName, false, Encoding.UTF8))
      {
        sw.WriteLine(JsonConvert.SerializeObject(result));
      }
    }
  }
}
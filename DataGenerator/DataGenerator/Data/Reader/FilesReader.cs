using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonLibraries.Extensions;
using DataGenerator.Data.Reader.Objects;
using DataGenerator.Data.VkCrawler;
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
          if (line != string.Empty)
          {
            line = line.Trim().Split(' ').ElementAt(2);
            result.Add(line);
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

    public List<VkCity> ReadCities(string path)
    {
      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        return JsonConvert.DeserializeObject<List<VkUserData>>(sr.ReadToEnd()).Where(x => x.City != null)
          .OrderBy(x => x.City.CityId)
          .Select(x => x.City).ToList();
      }
    }

    public List<VkUserData> ReadUsers(string path)
    {
      using (var sr = new StreamReader(path))
      {
        return JsonConvert.DeserializeObject<List<VkUserData>>(sr.ReadToEnd());
      }
    }

    /// <summary>
    ///   для каждого 100 000 выборки формировать свой файл собственный. так как id по убыванию, то  можно подумать, что
    ///   сначала молодые будут. Однако распределение позволяет сказать, что так может быть не всегда. Сохранять построчно
    ///   кажый объект
    /// </summary>
    /// <param name="folderFromPath"></param>
    /// <param name="pattern"></param>
    /// <param name="folderPathTo"></param>
    /// <param name="fileName"></param>
    public void ProcessVkUsers(string folderFromPath, string pattern, string folderPathTo, string fileName)
    {
      var result = new List<VkUserData>();

      var files = Directory.GetFiles(folderFromPath).Where(x => x.Contains(pattern)).ToList();
      for (var i = 0; i < files.Count / 100; i++)
      {
        var curFiles = files.Skip(i * 100).Take(100).ToList();
        foreach (var file in curFiles)
          using (var sr = new StreamReader(file))
          {
            var json = sr.ReadToEnd().Replace("No", "").Replace("Name", "");
            var jsonUsers = JsonConvert.DeserializeObject<VkUserDataResponse>(json).Response.Items
              .Where(x => x.Birthday.Age() >= 10 && x.Birthday.Age() <= 25  && !x.LargePhoto.Contains("deactivated") && x.City != null && !x.LargePhoto.Contains("camera")).Select(x=>
                {
                  x.City.Title = x.City.Title.Replace("'", "''");
                  return x;
                }).OrderBy(x => x.Birthday.Age()).ToList();
            result.AddRange(jsonUsers);
          }
        using (var sw =
          new StreamWriter(folderPathTo + Path.GetFileNameWithoutExtension(fileName) + $"_{i}" + Path.GetExtension(fileName), false,
            Encoding.UTF8))
        {
          sw.WriteLine(JsonConvert.SerializeObject(result));
        }
        result.Clear();
      }


    }

    //public void CombineVkUsers(string folderFromPath, string pattern, string folderPathTo, string fileName)
    //{
    //  var result = new List<VkUserData>();

    //  var files = Directory.GetFiles(folderFromPath).Where(x => x.Contains(pattern)).ToList();
    //  foreach (var file in files)
    //    using (var sr = new StreamReader(file))
    //    {
    //      using (var sw = new StreamWriter(folderPathTo + fileName, false, Encoding.UTF8))
    //      {
    //        var json = sr.ReadToEnd().Replace("29.2", "25.2").Replace("32.5", "30.5").Replace("99.1", "27.1")
    //          .Replace("-27.1", "27.1").Replace("No", "").Replace("Name", "");
    //        var jsonUsers = JsonConvert.DeserializeObject<VkUserDataResponse>(json).Response.Items
    //          .Where(x => x.Birthday.Age() > 5).ToList();

    //        foreach (var item in jsonUsers)
    //        {

    //          sw.WriteLine(JsonConvert.SerializeObject(item));
    //        }
    //      }
    //    }

    //}
  }
}
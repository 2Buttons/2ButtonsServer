using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonLibraries.Extensions;
using DataGenerator.Data.Reader.Objects;
using DataGenerator.Data.VkCrawler;
using DataGenerator.ScriptsGenerators.DirectInsertion.Entities;
using Independentsoft.Office.Spreadsheet;
using Newtonsoft.Json;

namespace DataGenerator.Data.Reader
{
  public class FilesManager
  {
    //public const string QuestionsUrl =
    //  @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\Questions.xlsx";

    //public const string VkUsersUrl = @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\VkUsers";

    //public const string FolderUrl = @"E:\Projects\2Buttons\Project\VkUsers\";

    public List<string> ReadMainCities(string path)
    {
      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        var json = sr.ReadToEnd();
        return JsonConvert.DeserializeObject<VkCityResponse>(json).Response.Items
          .Select(x => x.Title.Trim().Replace("'","''")).ToList();
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
      }

      return result;
    }

    public List<QuestionReader> ReadQuestions(string path)
    {
      var result = new List<QuestionReader>();
      var book = new Workbook(path);
      var sheets = book.Sheets;
      var questionSheet = sheets.FirstOrDefault(x => x.Name == "Переформулировка");

      if (questionSheet is Worksheet worksheet)
      {
        var cells = worksheet.GetCells();

        for (var i = 0; i < cells.Count; i++)
        {
          var questionId = int.Parse(cells[i].Value);
          var condition = cells[i + 1].Value.Replace("'", "''");
          var firstOption = cells[i + 2].Value.Replace("'", "''");
          var firstAnswersPercent = (int)double.Parse(cells[i + 3].Value.Replace('.', ','));
          var tags = (cells[i + 4].Value).Split(',').Select(x => x.Trim().ToUpper().Replace("'", "''")).ToList();
          var secondOption = cells[i + 7].Value.Replace("'", "''");
          var secondAnswersPercent = (int)double.Parse(cells[i + 8].Value.Replace('.', ','));
          var question = new QuestionReader
          {
            QuestionId = questionId,
            Condition = condition,
            FirstOption = firstOption,
            FirstAnswersPercent = firstAnswersPercent,
            Tags = tags,
            SecondOption = secondOption,
            SecondAnswersPercent = secondAnswersPercent
          };
          result.Add(question);
          i = i + 9;
        }
      }
      return result;
    }

    public List<QuestionDescriptionReader> ReadQuestionDescrition(string path)
    {
      var result = new List<QuestionDescriptionReader>();
      var book = new Workbook(path);
      var sheets = book.Sheets;
      var questionSheet = sheets.FirstOrDefault(x => x.Name == "Картинки");

      if (questionSheet is Worksheet worksheet)
      {
        var cells = worksheet.GetCells();

        for (var i = 0; i < cells.Count; i++)
        {

          var url = cells[i].Value;
          var tags = (cells[i + 1].Value).Split(',').Select(x => x.Trim().ToUpper().Replace("'", "''")).ToList();

          List<int> ids = new List<int>();

          for (int j = 0; j < 5; j++)
          {
            if (int.TryParse(cells[i + 2 + j].Value, out var id))
            {
              ids.Add(id);
            }
          }
          var question = new QuestionDescriptionReader
          {
            BackgroundUrl = url,
            Tags = tags,
            QuestionIds = ids
          };
          result.Add(question);
          i = i + 6;
        }
      }
      return result;
    }


    public List<UserReader> ReadUsers(string path)
    {
      using (var sr = new StreamReader(path))
      {
        return JsonConvert.DeserializeObject<List<UserReader>>(sr.ReadToEnd());
      }
    }

    /// <summary>
    ///   для каждого 100 000 выборки формировать свой файл собственный. так как id по убыванию, то  можно подумать, что
    ///   сначала молодые будут. Однако распределение позволяет сказать, что так может быть не всегда. Сохранять построчно
    ///   кажый объект
    /// </summary>
    /// <param name="folderPathFrom"></param>
    /// <param name="pattern"></param>
    /// <param name="folderPathTo"></param>
    /// <param name="fileName"></param>
    public void ProcessVkUsers(string folderPathFrom, string pattern, string folderPathTo, string fileName)
    {
      var result = new List<UserReader>();

      var files = Directory.GetFiles(folderPathFrom).Where(x => x.Contains(pattern)).ToList();
      int k = 0;
      for (var i = 0; i < files.Count; i++)
      {
        var file = files[i];
        using (var sr = new StreamReader(file))
        {
          var json = sr.ReadToEnd().Replace("No", "").Replace("Name", "");
          var jsonUsers = JsonConvert.DeserializeObject<VkUserDataResponse>(json).Response.Items
            .Where(x => x.Birthday.Age() >= 10 && x.Birthday.Age() <= 25 && !x.LargePhoto.Contains("deactivated") &&
                        x.City != null && !x.LargePhoto.Contains("camera")).Select(x => new UserReader
                        {
                          ExternalId = x.UserId,
                          Birthday = x.Birthday,
                          City = x.City.Title.Replace("'", "''"),
                          FirstName = x.FirstName.Replace("'", "''"),
                          LastName = x.LastName.Replace("'", "''"),
                          SexType = x.Sex,
                          VkOriginalAvatrUrl = x.LargePhoto
                        }).ToList();
          RandomizerExtension.Shuffle(jsonUsers);
          RandomizerExtension.Shuffle(jsonUsers);
          result.AddRange(jsonUsers);
        }


        if (result.Count >= 10_000)
        {
          using (var sw =
          new StreamWriter(folderPathTo + Path.GetFileNameWithoutExtension(fileName) + $"_{k++}" + Path.GetExtension(fileName), false,
            Encoding.UTF8))
          {
            sw.WriteLine(JsonConvert.SerializeObject(result.Take(10_000)));
          }
          result = result.Skip(10_000).ToList();
        }
        else
        if (i == files.Count - 1)
        {
          using (var sw =
            new StreamWriter(folderPathTo + Path.GetFileNameWithoutExtension(fileName) + $"_{k++}" + Path.GetExtension(fileName), false,
              Encoding.UTF8))
          {
            sw.WriteLine(JsonConvert.SerializeObject(result.Take(10_000)));
          }
        }

      }

      result.Clear();



    }


    public void SaveUrlsMatching(string path, List<UrlMatching> urls)
    {
      using (var sw = new StreamWriter(path, true, Encoding.UTF8))
      {
        foreach (var url in urls)
        {
          sw.WriteLine(JsonConvert.SerializeObject(url));
        }

      }
    }

    public void SaveUrlMatching(string path, UrlMatching url)
    {
      using (var sw = new StreamWriter(path, true, Encoding.UTF8))
      {
        sw.WriteLine(JsonConvert.SerializeObject(url));
      }
    }

    public List<UrlMatching> ReadUrlMatching(string path)
    {
      List<UrlMatching> matchings = new List<UrlMatching>();
      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        string line;
        while ((line = sr.ReadLine()) != null)
        {
          matchings.Add(JsonConvert.DeserializeObject<UrlMatching>(line));
        }
        return matchings;
      }
    }


  }
}
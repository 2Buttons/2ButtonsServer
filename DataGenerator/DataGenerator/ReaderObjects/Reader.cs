using System.Collections.Generic;
using System.IO;
using System.Text;
using Independentsoft.Office.Spreadsheet;

//using Microsoft.Office.Interop.Excel;

namespace DataGenerator.ReaderObjects
{
  public class Reader
  {
    public List<string> ReadCities(string path)
    {
      var result = new List<string>();

      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        string line;
        var index = 0;
        while ((line = sr.ReadLine()) != null) if (!string.IsNullOrEmpty(line)) result.Add(line.Trim());
      }
      return result;
    }

    public List<string> ReadMaleNames(string path)
    {
      var result = new List<string>();

      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        string line;
        while ((line = sr.ReadLine()) != null) if (!string.IsNullOrEmpty(line)) result.Add(line.Trim());
      }
      return result;
    }

    public List<string> ReadFemaleNames(string path)
    {
      var result = new List<string>();

      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        string line;
        while ((line = sr.ReadLine()) != null) if (!string.IsNullOrEmpty(line)) result.Add(line.Trim());
      }
      return result;
    }

    //public List<Surname> ReadSurnames(string path)
    //{
    //  var result = new List<Surname>();

    //  using (var sr = new StreamReader(path, Encoding.UTF8))
    //  {
    //    string line;
    //    while ((line = sr.ReadLine()) != null)
    //      if (!string.IsNullOrEmpty(line))
    //      {
    //        var surname = new Surname {Title = line.Trim()};
    //        result.Add(surname);
    //      }
    //  }

    //  for (int i = 1; i < result.Count; i++)
    //  {
    //    if (result[i-1].Title.Substring(0)== result[i].Title.Substring(0, result[i].Title.Length - 1))
    //    {
    //      result[i - 1].Gender = Gender.Male;
    //      result[i].Gender = Gender.Female;
    //    }
    //    else
    //    {
    //      if(result[i - 1].Gender == Gender.Both) result[i - 1].Gender = Gender.Both;
    //      result[i].Gender = Gender.Both;
    //    }
    //  }
    //  return result;
    //}

    //private void SetGender(Surname first, Surname second)
    //{
    //  if (first.Gender == Gender.Male || first.Gender == Gender.Female)
    //  {
    //    second.Gender = Gender.Both;
    //    return;
    //  }



    //}

    public List<Question> ReadQuestions(string path)
    {
      var result = new List<Question>();
      var book = new Workbook(path);

      foreach (var sheet in book.Sheets)
        if (sheet is Worksheet)
        {
          var worksheet = (Worksheet) sheet;
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
            //var m = question;
            // Console.WriteLine(cells[i].Reference + " = " + cells[i].Value);
          }
        }
      return result;

      //var excelApp = new Application();
      //excelApp.Visible = true; // Отвечает за то, будет ли видимо приложен
      //excelApp.Workbooks.Open(path);
      //var row = 1;
      //var maping = new List<List<string>>();
      //var currentSheet = (Worksheet) excelApp.Workbooks[1].Worksheets[1];
      //while (currentSheet.get_Range("A" + row).Value2 != null)
      //{
      //  var tempList = new List<string>();
      //  for (var column = 'A'; column < 'D'; column++)
      //  {
      //    var cell = currentSheet.get_Range(column + row);
      //    tempList.Add(cell != null ? cell.Value2.ToString() : "");
      //  }
      //  maping.Add(tempList);
      //  row++;
      //}

      //var t = maping;
      //Console.WriteLine(t.Count);
      //return new List<City>();
    }
  }
}
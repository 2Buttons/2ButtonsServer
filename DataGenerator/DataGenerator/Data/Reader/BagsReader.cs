using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataGenerator.Data.ReaderObjects;
using Newtonsoft.Json;

namespace DataGenerator.Data.Reader
{
  public class BagsReader
  {
    public List<City> ReadCities(string path)
    {
      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        return JsonConvert.DeserializeObject<List<City>>(sr.ReadToEnd()).ToList();
      }
    }

    public List<User> ReadUsers(string path)
    {
      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        return JsonConvert.DeserializeObject<List<User>>(sr.ReadToEnd()).ToList();
      }
    }

    public List<Question> ReadQuestions(string path)
    {
      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        return JsonConvert.DeserializeObject<List<Question>>(sr.ReadToEnd()).ToList();
      }
    }
  }
}
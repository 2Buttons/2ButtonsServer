using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DataGenerator.VkCrawler
{
  public class UsersManaging
  {

    public void GetUsersFromVkFiles(string folderPath)
    {

      var result = new List<VkUserData>();

      var files = Directory.GetFiles(folderPath).Where(x=>x.Contains("VkUsers")).ToList();
      foreach (var file in files)
      {
        using (StreamReader sr = new StreamReader(file))
        {
          var json = sr.ReadToEnd().Replace("29.2","25.2").Replace("32.5","30.5");
          var jsonUsers = JsonConvert.DeserializeObject<VkUserDataResponse>(json).Response
            .Items.ToList();
          result.AddRange(jsonUsers);
        }
      }

      using (var sw = new StreamWriter(folderPath+"Users.txt", false, Encoding.UTF8))
      {
        sw.WriteLine(JsonConvert.SerializeObject(result));
      }
    }

  }
}

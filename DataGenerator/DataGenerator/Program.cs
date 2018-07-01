using System;
using System.IO;
using System.Threading;
using CommonLibraries.SocialNetworks.Vk;
using DataGenerator.ReaderObjects;

namespace DataGenerator
{
  internal class Program
  {
    public const string CitiesUrl = @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\Cities.txt";

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

    private static void Main(string[] args)
    {
      var reader = new Reader();
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


      var AppId = "6469856";
      var AppSecret = "Zcec8RyHjpvaVfvRxLvq";
      var AppAccess = "8ca841528ca841528ca84152578ccaf9b288ca88ca84152d645d207a5fae99916d1944a";

      VkService service = new VkService();

      //var users = service.GetUsersFromGroup(1,AppAccess).GetAwaiter().GetResult();


      //using (StreamWriter sw = new StreamWriter(VkUsersUrl, false, System.Text.Encoding.UTF8))
      //{
      //  foreach (var user in users)
      //  {
      //    sw.WriteLine(Json.Ser)
      //  }
      //}




      var groupId = "opros_tyta";

      for (int i = 0; i < 100; i++)
      {
        var path = VkUsersUrl + "_"+groupId + $"_{i}_" + ".txt";
        var users = service.GetJsonFromGroup(groupId, i, AppAccess).GetAwaiter().GetResult();
        using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.UTF8))
        {

          sw.WriteLine(users);

        }
        Thread.Sleep(4000);
      }
     
     



    }
  }
}
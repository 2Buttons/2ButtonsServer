using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLibraries;
using DataGenerator.ScriptsGenerators.DirectInsertion.Entities;
using DataGenerator.ScriptsGenerators.FunctionInsertion.Queries;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;

namespace DataGenerator
{
  internal class Program
  {
    public const string CitiesUrl = @"E:\Projects\2Buttons\Project\Data\Bags\Cities.txt";
    public const string CitiesMatchingUrl = @"E:\Projects\2Buttons\Project\Data\Bags\CitiesMatching.txt";

    public const string QuestionsUrl = @"E:\Projects\2Buttons\Project\Data\Files\Questions.xlsx";

    public const string MaleNamesUrl =
      @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\MaleNames.txt";

    public const string FemaleNamesUrl =
      @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\FemaleNames.txt";

    public const string SurnamesUrl =
      @"E:\Projects\2Buttons\Project\Server\DataGenerator\DataGenerator\Files\Surnames.txt";

    public const string VkUsersUrl = @"E:\Projects\2Buttons\Project\Data\Files\Users_0.txt";

    public const string FolderUrl = @"E:\Projects\2Buttons\Project\Data\Bags\";

    public static void ResizeImage(string originalPath, string rezisePath, int height, int width)
    {
      using (var image = Image.Load(originalPath))
      {
        image.Mutate(x => x.Resize(new ResizeOptions
        {
          Mode = ResizeMode.Max,
          Size = new SixLabors.Primitives.Size { Height = height, Width = width }
        }));
        image.Save(rezisePath); // Automatic encoder selected based on extension.
      }
    }


    private static void Main(string[] args)
    {
      //string fileName = "Viper.jpg";
      //string originalFilePath = @"E:\Projects\2Buttons\Project\Data\Content\";
      //var smallImagePath = @"E:\Projects\2Buttons\Project\Data\Content\"+"SMALL_"+ fileName;
      //var largemagePath = @"E:\Projects\2Buttons\Project\Data\Content\" + "LARGE_" + fileName;

      //ResizeImage(originalFilePath+fileName, Path.Combine(smallImagePath), 100, 100);
      //ResizeImage(originalFilePath+ fileName, Path.Combine(largemagePath), 600, 600);

      var questionIdOffset = 100;
      var userIdOffset = 100;

      var manager = new Manager();
      var avatarsMatchingBag = manager.ReadAvatarsMatching();
      var backgroundsMatchingBag = manager.ReadBackgroundsMatching();

      var mainCities = manager.ReadMainCities();

      var questionsDescriptionsBag = manager.ReadQuestionDescription();
      var usersBag = manager.ReadUsers(0, 1007); //1007
      var questionsBag = manager.ReadQuestions();

      // users

      var users = manager.FromUserReaderToUer(userIdOffset, usersBag, avatarsMatchingBag);
      var questions = manager.FromQuestionReaderToQuestion(questionIdOffset, questionsBag, questionsDescriptionsBag,
        backgroundsMatchingBag);
      manager.DistributeQuestionOwners(users, questions);

      var cityQueries = manager.CreateMainCities(mainCities);

      var answerQueries = manager.CreateAnswers(users, questions);
      var questionFeedbacksQueries = manager.CreateFeedbacks(users, questions);

      var followerQueries = manager.CreateFollowings(users);

      var tagQueries = manager.CreateTags(questions);
      var userQueries = manager.CreateUsers(users);
      var questionQueries = manager.CreateQuestions(questions);


      var answersAndFeedbacksEntities = manager.CreateAnswersEntities(answerQueries, questionFeedbacksQueries);

      manager.CreateScripts(cityQueries, userQueries.userInfos.OrderBy(x=>x.UserId).ToList(), userQueries.users.OrderBy(x => x.UserId).ToList(), followerQueries.OrderBy(x => x.UserId).ToList(),
        questionQueries.OrderBy(x => x.QuestionId).ToList(), tagQueries.OrderBy(x => x.QuestionId).ToList(), answersAndFeedbacksEntities);// answerQueries, questionFeedbacksQueries);



      Console.ReadLine();
    }

   
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DataGenerator.ScriptsGenerators.DirectInsertion.Entities;
using DataGenerator.ScriptsGenerators.FunctionInsertion.Queries;

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

    private static void Main(string[] args)
    {
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

      var followerQueries = manager.CreateFollowers(users);

      var tagQueries = manager.CreateTags(questions);
      var userQueries = manager.CreateUsers(users);
      var questionQueries = manager.CreateQuestions(questions);


      var answersAndFeedbacksEntities = manager.CreateAnswersEntities(answerQueries, questionFeedbacksQueries);
      var followersEntities = manager.CreateFollowersEntities(followerQueries);

      manager.CreateScripts(cityQueries, userQueries.userInfos, userQueries.users, followersEntities,
        questionQueries, tagQueries, answersAndFeedbacksEntities);// answerQueries, questionFeedbacksQueries);

      //

      Console.ReadLine();
    }

   
  }
}
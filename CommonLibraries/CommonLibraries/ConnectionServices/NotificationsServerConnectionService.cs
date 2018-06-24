using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries.Response;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CommonLibraries.Helpers
{
  public class NotificationsServerHelper
  {
    private readonly ILogger<MediaServerHelper> _logger;

    private readonly string _followNotificationUrl; //= "http://localhost:16009/notifications/internal/follow";
    private readonly string _commentNotificationUrl;// = "http://localhost:16009/notifications/internal/comment";
    private readonly string _recommendQuestionNotificationUrl;// = "http://localhost:16009/notifications/internal/recommendQuestion";

    public NotificationsServerHelper(IOptions<ServersSettings> options, ILogger<MediaServerHelper> logger)
    {
      _logger = logger;
      _followNotificationUrl = $"http://localhost:{options.Value["Notifications"].Port}/notifications/internal/follow";
      _commentNotificationUrl = $"http://localhost:{options.Value["Notifications"].Port}/notifications/internal/comment";
      _recommendQuestionNotificationUrl = $"http://localhost:{options.Value["Notifications"].Port}/notifications/internal/recommendQuestion";
    }


    public  Task SendFollowNotification(int notifierId, int followToId, DateTime followedDate)
    {
      _logger.LogInformation($"{nameof(SendFollowNotification)}.Start");
      var body = JsonConvert.SerializeObject(new
      {
        NotifierId = notifierId,
        FollowToId = followToId,
        FollowedDate = followedDate
      });

      var result =  Task.Factory.StartNew(()=>NotificationsServerConnection(_followNotificationUrl, body));
      _logger.LogInformation($"{nameof(SendFollowNotification)}.End");
      return result;
    }

    public   Task SendCommentNotification(int notifierId, int questionId, int commentId,
      DateTime commentedDate)
    {
      _logger.LogInformation($"{nameof(SendCommentNotification)}.Start");
      var body = JsonConvert.SerializeObject(new
      {
        NotifierId = notifierId,
        QuestionId = questionId,
        CommentId = commentId,
        CommentedDate = commentedDate
      });

      var result =  Task.Factory.StartNew(() => NotificationsServerConnection(_commentNotificationUrl, body));
      _logger.LogInformation($"{nameof(SendCommentNotification)}.End");
      return result;
    }

    public  Task SendRecommendQuestionNotification(int notifierId, int userToId, int questionId,
      DateTime recommendedDate)
    {
      _logger.LogInformation($"{nameof(SendRecommendQuestionNotification)}.Start");
      var body = JsonConvert.SerializeObject(new
      {
        NotifierId = notifierId,
        UserToId = userToId,
        QuestionId = questionId,
        RecommendedDate = recommendedDate
      });

      var result =  Task.Factory.StartNew(() => NotificationsServerConnection(_recommendQuestionNotificationUrl, body));
      _logger.LogInformation($"{nameof(SendRecommendQuestionNotification)}.End");
      return result;
    }

    private static async Task<object> NotificationsServerConnection(string url, string body)
    {
      var request = WebRequest.Create(url);
      request.Method = "POST";
      request.ContentType = "application/json";
      using (var requestStream = request.GetRequestStream())
      using (var writer = new StreamWriter(requestStream))
      {
        writer.Write(body);
      }
      var webResponse = await request.GetResponseAsync();
      using (var responseStream = webResponse.GetResponseStream())
      using (var reader = new StreamReader(responseStream))
      {
        var result = reader.ReadToEnd();
        return MediaResponseToUrl(result);
      }
    }

    private static object MediaResponseToUrl(string response)
    {
      return JsonConvert.DeserializeObject<ResponseObject>(response).Data;
    }
  }
}
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries.Response;
using Newtonsoft.Json;

namespace CommonLibraries.Helpers
{
  public class NotificationServerHelper
  {
    private const string FollowNotificationUrl = "http://localhost:16080/notifications/internal/follow";
    private const string CommentNotificationUrl = "http://localhost:16080/notifications/internal/comment";
    private const string RecommendQuestionNotificationUrl = "http://localhost:16080/notifications/internal/recommendQuestion";

    public static Task SendFollowNotification(int notifierId, int followToId, DateTime followedDate)
    {
      var body = JsonConvert.SerializeObject(new
      {
        NotifierId = notifierId,
        FollowToId = followToId,
        FollowedDate = followedDate
      });

      return Task.Factory.StartNew(()=>NotificationsServerConnection(FollowNotificationUrl, body));
    }

    public static  Task SendCommentNotification(int notifierId, int questionId, int commentId,
      DateTime commentedDate)
    {
      var body = JsonConvert.SerializeObject(new
      {
        NotifierId = notifierId,
        QuestionId = questionId,
        CommentId = commentId,
        CommentedDate = commentedDate
      });

      return Task.Factory.StartNew(() => NotificationsServerConnection(CommentNotificationUrl, body));
    }

    public static Task SendRecommendQuestionNotification(int notifierId, int userToId, int questionId,
      DateTime recommendedDate)
    {
      var body = JsonConvert.SerializeObject(new
      {
        NotifierId = notifierId,
        UserToId = userToId,
        QuestionId = questionId,
        RecommendedDate = recommendedDate
      });

      return Task.Factory.StartNew(() => NotificationsServerConnection(RecommendQuestionNotificationUrl, body));
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
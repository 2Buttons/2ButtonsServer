using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CommonLibraries.Helpers
{
  public class MonitoringServerHelper
  {
    private const string MonitoringServerUrl = "http://localhost:6570/internal/follow";

    public static async Task SendMonitoringInfo(int userId, UserMonitoringType monitoringType)
    {
      var body = JsonConvert.SerializeObject(new {userId, monitoringType});

      await MonitoringServerConnection(MonitoringServerUrl, body);
    }

    private static async Task<object> MonitoringServerConnection(string url, string body)
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
        return result;
      }
    }
  }
}
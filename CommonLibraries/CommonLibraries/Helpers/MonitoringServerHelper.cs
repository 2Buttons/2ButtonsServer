using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CommonLibraries.Helpers
{
  public class MonitoringServerHelper
  {
    private const string AddMonitoringServerUrl = "http://localhost:17009/monitoring/add";
    private const string UpdateMonitoringServerUrl = "http://localhost:17009/monitoring/update";

    public static Task AddUrlMonitoring(int userId)
    {
      var body = JsonConvert.SerializeObject(new {userId});

      return Task.Factory.StartNew(() => MonitoringServerConnection(AddMonitoringServerUrl, body));
    }

    public static Task UpdateUrlMonitoring(int userId, UrlMonitoringType monitoringType)
    {
      var body = JsonConvert.SerializeObject(new { userId, UrlMonitoringType = monitoringType });

      return Task.Factory.StartNew(()=> MonitoringServerConnection(UpdateMonitoringServerUrl, body));
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
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CommonLibraries.Helpers
{
  public class MonitoringServerConnection
  {

    private readonly ILogger<MediaServerConnection> _logger;

    private readonly string _addMonitoringServerUrl;
    private readonly string _updateMonitoringServerUrl;

    public MonitoringServerConnection(IOptions<ServersSettings> mediaOptions, ILogger<MediaServerConnection> logger)
    {
      _logger = logger;
      _addMonitoringServerUrl = $"http://localhost:{mediaOptions.Value["Monitoring"].Port}/monitoring/add";
      _updateMonitoringServerUrl = $"http://localhost:{mediaOptions.Value["Monitoring"].Port}/monitoring/update";
    }

    public Task AddUrlMonitoring(int userId)
    {
      _logger.LogInformation($"{nameof(AddUrlMonitoring)}.Sart");
      var body = JsonConvert.SerializeObject(new {userId});
      var result = Task.Factory.StartNew(() => MonitoringServerConnect(_addMonitoringServerUrl, body));
      _logger.LogInformation($"{nameof(AddUrlMonitoring)}.End");

      return result;
    }

    public Task UpdateUrlMonitoring(int userId, UrlMonitoringType monitoringType)
    {
      _logger.LogInformation($"{nameof(UpdateUrlMonitoring)}.Sart");
      var body = JsonConvert.SerializeObject(new { userId, UrlMonitoringType = monitoringType });
      var result =  Task.Factory.StartNew(()=> MonitoringServerConnect(_updateMonitoringServerUrl, body));
      _logger.LogInformation($"{nameof(UpdateUrlMonitoring)}.End");

      return result;
    }

    private async Task<object> MonitoringServerConnect(string url, string body)
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
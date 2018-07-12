using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DataViewer.ViewModels;
using Newtonsoft.Json;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace DataViewer
{
  /// <summary>
  /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var questions =  GetQuestions();
      
      lstQuestions.ItemsSource = questions.Select((x)=>
      {
        int first = x.Options.First().Voters;
        int second = x.Options.Last().Voters;
        if (second + first == 0) first = 1;

        return "Id: " + x.QuestionId + " | f: " + first + "=" + (first/(first + second)*100) + "%  | s:" + second + "=" +
               (second/(first + second)*100)+"%";
      }).ToList();

    }

    public List<QuestionViewModel> GetQuestions()
    {
      string url = "http://localhost:18000/TwoButtons1234567890TwoButtons/bots/questions";
      string body = JsonConvert.SerializeObject(new GetQuestionsViewModel
      {
        Code = "MySecretCode!123974_QQ",
        PageParams = new PageParams {Count = 1000, Offset = 0}
      });
      var response = JsonConvert.DeserializeObject<ResponseObject<List<QuestionViewModel>>>(Request(body, url));
      tbServerResponce.Text = "Code: " + response.Status + " Message:" + response.Message;
      var result = response.Data;
      return result;
    }

    public string Request(string body, string url)
    {
      var request = WebRequest.Create(url);
      request.Method = "POST";
      request.ContentType = "application/json";
      using (var requestStream = request.GetRequestStreamAsync().GetAwaiter().GetResult())
      using (var writer = new StreamWriter(requestStream))
      {
        writer.Write(body);
      }
      var webResponse = request.GetResponseAsync().GetAwaiter().GetResult();
      using (var responseStream = webResponse.GetResponseStream())
      using (var reader = new StreamReader(responseStream))
      {
        var result = reader.ReadToEnd();
        return result;
      }
    }

    public string GetServerVersion()
    {
      string path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
      using (StreamReader sr = new System.IO.StreamReader(new FileStream(path, FileMode.Open)))
      {
        var file = sr.ReadToEnd();
        return JsonConvert.DeserializeObject<AppSettings>(file).ServerVersion;
      }
    }

    private void btnVote_Click(object sender, RoutedEventArgs e)
    {
      string url = "http://localhost:18000/TwoButtons1234567890TwoButtons/bots/magic";
      string body = JsonConvert.SerializeObject(new MagicViewModel
      {
        Code = "MySecretCode!123974_QQ",
         BotsCount = int.Parse(tbBotsCount.Text),
           FirstOptionPercent = int.Parse(tbOption1Voters.Text),
           SecondOptionPercent = int.Parse(tbOption2Voters.Text),
            VoteDurationInMilliseconds = int.Parse(tbInterval.Text)*1000,
             QuestionId = int.Parse(tbId.Text)
      });
      var response = JsonConvert.DeserializeObject<ResponseObject<object>>(Request(body, url));
      tbServerResponce.Text = "Code: " + response.Status + " Message:" + response.Message;
     // var result = response.Data;
     // return result;
    }
  }
}



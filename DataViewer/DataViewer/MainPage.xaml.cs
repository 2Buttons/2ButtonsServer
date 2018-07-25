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
    public List<QuestionViewModel> Questions { get; set; }

    public MainPage()
    {
      this.InitializeComponent();
      tbOption1Voters.TextChanged += TbOption1Voters_TextChanged;
      tbOption2Voters.TextChanged += TbOption2Voters_TextChanged;
      lstQuestions.SelectionChanged += LstQuestions_SelectionChanged;
      tbId.TextChanged += TbId_TextChanged;
    }

    private void TbId_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (Questions == null || Questions.Count == 0 || string.IsNullOrEmpty(((TextBox)sender).Text))
      {
        tBlockIdError.Text = "";
        return;
      }

      if (!int.TryParse(((TextBox)sender).Text, out var questionId))
      {
        questionId = 0;
      }
     
      if (!Questions.Any(x => x.QuestionId == questionId))
      {
        tBlockIdError.Text = "id не найден";
        return;
      }

      tBlockIdError.Text = "";
    }

    private void LstQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var value = (string)((ListView) sender).SelectedItem ?? "";
      if (!string.IsNullOrEmpty(value.Trim()))
      {
        var elements = value.Split(' ');
        var id = int.Parse(elements[1]);
        tbId.Text = id.ToString();
        var option1 = int.Parse(elements[6]);
        var option2 = int.Parse(elements[12]);
        tbOption1Voters.Text = option1.ToString();
        tbOption2Voters.Text = option2.ToString();
      }
    }

    private void TbOption2Voters_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!int.TryParse(((TextBox)sender).Text, out var secondVoters))
      {
        secondVoters = 0;
      }
      tbOption1Voters.Text = (100 - secondVoters).ToString();
    }

    private void TbOption1Voters_TextChanged(object sender, TextChangedEventArgs e)
    {

      if(!int.TryParse(((TextBox)sender).Text,out var firstVoters))
      {
        firstVoters = 0;
      }
      tbOption2Voters.Text = (100 - firstVoters).ToString();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var questions =  GetQuestions();
      Questions = questions;


      lstQuestions.ItemsSource = questions.Select((x)=>
      {
        int first = x.Options.First().Voters;
        int second = x.Options.Last().Voters;
        if (second + first == 0) first = 1;

        return "Id: " + x.QuestionId + " | f: " + first + " = " + (first * 100 / (first + second)) + " %  | s:" + second + " = " +
               (second * 100  / (first + second))+" %";
      }).ToList();

      tBlockQCount.Text = "Вопросы: "+Questions.Count.ToString();
      tBlockBCount.Text = "Боты: "+GetBotsCount();
    }

    public List<QuestionViewModel> GetQuestions()
    {
      var version = GetServerVersion();
      string url = $"https://api.2buttons.ru/{version}/TwoButtons1234567890TwoButtons/bots/questions";
      //string url = "https://localhost:18000/TwoButtons1234567890TwoButtons/bots/questions";
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

    public int GetBotsCount()
    {
      var version = GetServerVersion();
      string url = $"https://api.2buttons.ru/{version}/TwoButtons1234567890TwoButtons/bots/count";
      //string url = "http://localhost:18000/TwoButtons1234567890TwoButtons/bots/count";
      string body = JsonConvert.SerializeObject(new GetBotsCount
      {
        Code = "MySecretCode!123974_QQ"
      });
      var response = JsonConvert.DeserializeObject<ResponseObject<int>>(Request(body, url));
     // tbServerResponce.Text = "Code: " + response.Status + " Message:" + response.Message;
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
      return "v0.13";
      //string path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
      //using (StreamReader sr = new System.IO.StreamReader(new FileStream(path, FileMode.Open)))
      //{
      //  var file = sr.ReadToEnd();
      //  return JsonConvert.DeserializeObject<AppSettings>(file).ServerVersion;
      //}
    }

    private void btnVote_Click(object sender, RoutedEventArgs e)
    {
      var version = GetServerVersion();
      string url = $"https://api.2buttons.ru/{version}/TwoButtons1234567890TwoButtons/bots/magic";
      //string url = "http://localhost:18000/TwoButtons1234567890TwoButtons/bots/magic";
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



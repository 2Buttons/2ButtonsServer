using System.Net;
using System.Net.Mail;

namespace AccountServer.Infrastructure.Services
{
  public class EmailService
  {
    public void SendMessage()
    {
      var client = new SmtpClient("smtp.yandex.ru", 25)
      {
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential("misha.tsoi2018@yandex.ru", "misha.tsoi20181234567890")
      };

      var mailMessage = new MailMessage {From = new MailAddress("misha.tsoi2018@yandex.ru", "Misha Tsoy")};
      mailMessage.To.Add("Yura290610@yandex.ru");
      mailMessage.Body = "Hello my first email)";
      mailMessage.Subject = "Просто тема";
      client.Send(mailMessage);
    }
  }
}
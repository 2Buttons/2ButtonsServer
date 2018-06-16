using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLibraries.EmailManager
{
  public class EmailSender
  {

    public void SendMessage()
    {
      SmtpClient client = new SmtpClient("smtp.yandex.ru", 587);
      client.UseDefaultCredentials = false;
      client.Credentials = new NetworkCredential("no-reply@2buttons.ru", "ELhobapIopkuer0");
      client.EnableSsl = true;

      MailMessage mailMessage = new MailMessage();
      mailMessage.From = new MailAddress("no-reply@2buttons.ru", "Администрация сайта");
      mailMessage.To.Add("Yura290610@yandex.ru");
      mailMessage.Body = "Hello my first email)";
      mailMessage.Subject = "Просто тема";
      client.Send(mailMessage);
    }

    public void SendMessage(IEnumerable<string> recipients)
    {
      MailMessage mailMessage = new MailMessage();
      mailMessage.From = new MailAddress("no-reply@2buttons.ru", "Администрация сайта");

      foreach (var recipient in recipients) mailMessage.To.Add(recipient);

      mailMessage.Body = "Hello my first email)";
      mailMessage.Subject = "Просто тема";
      client.Send(mailMessage);
    }

   
  }
}

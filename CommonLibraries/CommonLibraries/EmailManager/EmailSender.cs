﻿using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLibraries.EmailManager
{
  public class EmailSender
  {

    public  void SednNoReply(string recepient, string subject, string body)
    {
      var sender = new EmailOptions
      {
        Server = "smtp.yandex.ru",
        Port = (int)PopularPorts.YandexSmtp,
        SenderEmail = "no-reply@2buttons.ru",
        SenderName ="no-reply@2buttons.ru",
        Password =  "ELhobapIopkuer0"
      };
       SendMessage(sender, recepient, subject, body);
    }

    public void  SendMessage(EmailOptions senderOptions, string recepient, string subject, string body)
    {
       SendMessage(senderOptions, new List<string> {recepient},  subject,  body);
    }

    public async void  SendMessage(EmailOptions senderOptions, IEnumerable<string> recipients, string subject, string body)
    {


      MailMessage mailMessage = new MailMessage
      {
        From = new MailAddress(senderOptions.SenderEmail, senderOptions.SenderName),
        Body = body,
        Subject = subject,
        BodyEncoding = Encoding.UTF8,
        IsBodyHtml = true,
        Priority = MailPriority.High
      };

      foreach (var recipient in recipients) mailMessage.To.Add(recipient);

      using (var client = new SmtpClient(senderOptions.Server, senderOptions.Port)) 
      {
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(senderOptions.SenderEmail, senderOptions.Password);
        client.EnableSsl = senderOptions.EnablrSsl;
        await client.SendMailAsync(mailMessage);
      }
    }
  }
}

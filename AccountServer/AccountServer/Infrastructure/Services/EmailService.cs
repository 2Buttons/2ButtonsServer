using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AccountServer.Infrastructure.Services
{
    public class EmailService
    {

      public void SendMessage()
      {
        SmtpClient client = new SmtpClient("smtp.yandex.ru", 25);
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential("misha.tsoi2018@yandex.ru", "misha.tsoi20181234567890");





        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("misha.tsoi2018@yandex.ru", "Misha Tsoy");
        mailMessage.To.Add("Yura290610@yandex.ru");
        mailMessage.Body = "Hello my first email)";
        mailMessage.Subject = "Просто тема";
        client.Send(mailMessage);
    }
    }
}

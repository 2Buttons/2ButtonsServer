using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibraries.EmailManager
{
  public class EmailOptions
  {

    /// <summary>
    /// SMTP Server address
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// SMTP Server Port ,default is 25
    /// </summary>
    public int Port { get; set; } = 25;

    /// <summary>
    /// send user name
    /// </summary>
    public string SenderName { get; set; }

    /// <summary>
    /// send user email
    /// </summary>
    public string SenderEmail { get; set; }

    /// <summary>
    /// send user password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// enable SSL 
    /// </summary>
    public bool EnablrSsl { get; set; } = true;
  }

  public enum PopularPorts
  {
    YandexSmtp = 587
  }
}

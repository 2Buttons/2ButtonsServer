using System.ComponentModel.DataAnnotations;

namespace MediaServer.ViewModel
{
  public class UrlViewModel
  {
    [Required]
    public string Url { get; set; }
  }
}
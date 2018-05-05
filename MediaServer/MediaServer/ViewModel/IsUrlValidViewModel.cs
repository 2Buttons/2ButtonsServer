using System.ComponentModel.DataAnnotations;

namespace MediaServer.ViewModel
{
  public class IsUrlValidViewModel
  {
    [Required]
    public string Url { get; set; }
  }
}
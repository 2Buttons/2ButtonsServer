using System.ComponentModel.DataAnnotations;

namespace MediaServer.ViewModel
{
  public class UploadDefaultViaLinkViewModel
  {
    [Required]
    public string Url { get; set; }
  }
}
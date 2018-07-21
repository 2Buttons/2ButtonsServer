using System.ComponentModel.DataAnnotations;

namespace MediaServer.ViewModel
{
  public class UploadDefaultViaUrlViewModel
  {
    [Required]
    public string Url { get; set; }
  }
}
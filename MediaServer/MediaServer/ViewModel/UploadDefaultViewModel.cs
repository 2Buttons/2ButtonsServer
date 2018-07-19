using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using Microsoft.AspNetCore.Http;

namespace MediaServer.ViewModel
{
  public class UploadDefaultViewModel
  {
    [Required]
    public IFormFile File { get; set; }
  }
}
using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using Newtonsoft.Json;

namespace AccountServer.ViewModels.InputParameters
{
  public class AddSocialViewModel
  {
    [Required]
    [JsonProperty("state")]
    public string State { get; set; }

    [Required]
    [JsonProperty("social")]
    public SocialType SocialType { get; set; }

    [Required]
    [JsonProperty("status")]
    public bool Status { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("eror_description")]
    public string ErrorDescription { get; set; }
  }
}
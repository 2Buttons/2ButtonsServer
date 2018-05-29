using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using Newtonsoft.Json;

namespace AuthorizationServer.ViewModels.InputParameters.Auth
{
  public class ExternalLoginViewModel
  {
    [Required]
    [JsonProperty("state")]
    public string State { get; set; }

    [Required]
    public SocialType SocialType { get; set; }

    [Required]
    [JsonProperty("status")]
    public bool Status { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("erorDescription")]
    public string ErrorDescription { get; set; }
  }
}
using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using CommonTypes;
using Newtonsoft.Json;

namespace AuthorizationServer.ViewModels.InputParameters.Auth
{
  public class ExternalLoginViewModel
  {
    [JsonProperty("isTest")]
    public bool IsTest { get; set; } = false;

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
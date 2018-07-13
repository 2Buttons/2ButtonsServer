using System.ComponentModel.DataAnnotations;
using CommonLibraries;
using Newtonsoft.Json;

namespace AuthorizationServer.ViewModels.InputParameters.Auth
{
  public class ExternalLoginMobileViewModel
  {
    [Required]
    [JsonProperty("state")]
    public string State { get; set; }

    [Required]
    public SocialType SocialType { get; set; }

    [Required]
    [JsonProperty("externalUserId")]
    public long ExternalUserId { get; set; }

    [Required]
    [JsonProperty("accessToken")]
    public string AccessToken { get; set; }

    [Required]
    [JsonProperty("created")]
    public string Created { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [Required]
    [JsonProperty("expiresIn")]
    public long ExpiresIn { get; set; }

    [Required]
    [JsonProperty("isHttpsRequired")]
    public bool IsHttpsRequired { get; set; }

    [JsonProperty("secret")]
    public string Secret { get; set; }
  }
}
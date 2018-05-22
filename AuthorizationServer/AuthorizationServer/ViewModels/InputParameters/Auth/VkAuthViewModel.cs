using Newtonsoft.Json;

namespace AuthorizationServer.ViewModels.InputParameters.Auth
{
  public class VkAuthThokenViewModel
  {
    public bool Status { get; set; }
    public string AccessToken { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Error { get; set; }
    public string ErrorDescription { get; set; }
  }

  public class VkAuthCodeViewModel
  {
    [JsonProperty("state")]
    public string State { get; set; }
    [JsonProperty("status")]
    public bool Status { get; set; }
    [JsonProperty("code")]
    public string Code { get; set; }
    [JsonProperty("error")]
    public string Error { get; set; }
    [JsonProperty("eror_description")]
    public string ErrorDescription { get; set; }
  }

  public class VkAppAccessTokenCode1
  {
    [JsonProperty("user_id")]
    public int UserId { get; set; }
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
  }
}
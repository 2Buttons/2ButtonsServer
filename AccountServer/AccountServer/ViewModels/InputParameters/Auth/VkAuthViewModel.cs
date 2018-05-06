using Newtonsoft.Json;

namespace AccountServer.ViewModels.InputParameters.Auth
{
  public class VkAuthViewModel
  {
    public int UserId { get; set; }
    public string AccessToken { get; set; }
    public string Code { get; set; }
  }

  public  class VkAppAccessTokenCode
  {
    [JsonProperty("user_id")]
    public int UserId { get; set; }
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
  }
}
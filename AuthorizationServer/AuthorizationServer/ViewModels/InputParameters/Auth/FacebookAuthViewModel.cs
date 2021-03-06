﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AuthorizationServer.ViewModels.InputParameters.Auth
{

  public class FacebookAuthViewModel
  {
    [Required]
    [JsonProperty("state")]
    public string State { get; set; }
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

  public class FacebookAuthViewModel1
  {
    public string AccessToken { get; set; }
  }
}
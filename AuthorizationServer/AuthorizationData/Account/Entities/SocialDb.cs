﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonTypes;

namespace AuthorizationData.Account.Entities
{
  [Table("Socials")]
  public class SocialDb
  {
    [Key]
    public int SocialId { get; set; }

    public SocialType SocialType { get; set; }
    public int InternalId { get; set; }
    public long ExternalId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string ExternalToken { get; set; }
    public long ExpiresIn { get; set; }
  }
}
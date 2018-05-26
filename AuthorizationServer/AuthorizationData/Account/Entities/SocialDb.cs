using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CommonLibraries;

namespace AuthorizationData.Account.Entities
{
  [Table("Socials")]
  public class SocialDb
  {
    [Key]
    public int SocialId { get; set; }
    public SocialType SocialType { get; set; }
    public int InternalId { get; set; }
    public int ExternalId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string ExternalToken { get; set; }
    public int ExpiresIn { get; set; }
  }
}

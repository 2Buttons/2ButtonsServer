using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace AccountData.Account.Entities
{
  [Table("Socials")]
  public class SocialDb
  {
    [Key]
    public long SocialId { get; set; }

    public SocialType SocialType { get; set; }
    public long InternalId { get; set; }
    public long ExternalId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string ExternalToken { get; set; }
    public long ExpiresIn { get; set; }
  }
}
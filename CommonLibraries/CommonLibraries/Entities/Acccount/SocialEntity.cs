using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Entities.Acccount
{
  [Table("Socials")]
  public class SocialEntity
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
    public bool IsDeleted { get; set; }
  }
}
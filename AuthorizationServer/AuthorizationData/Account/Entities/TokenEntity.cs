using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibraries;

namespace AuthorizationData.Account.Entities
{
  [Table("Tokens")]
  public class TokenEntity
  {
    [Key]
    public long TokenId { get; set; }
    public int UserId { get; set; } // именно юзер
    public long ExpiresIn{ get; set; }
    public string RefreshToken { get; set; }

    public ApplicationType ApplicationType { get; set; } = ApplicationType.JavaScript;
    public string AllowedOrigin { get; set; } = string.Empty;
  }
}

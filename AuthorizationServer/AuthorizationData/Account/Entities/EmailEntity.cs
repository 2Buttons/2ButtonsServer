using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizationData.Account.Entities
{
  [Table("Emails")]
  public class EmailEntity
  {
    [Key]
    public int EmailId { get; set; }

    public string Email { get; set; }
  }
}
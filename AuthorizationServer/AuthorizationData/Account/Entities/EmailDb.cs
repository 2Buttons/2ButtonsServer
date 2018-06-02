using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizationData.Account.Entities
{
  [Table("Emails")]
  public class EmailDb
  {
    [Key]
    public int EmailId { get; set; }

    public string Email { get; set; }
  }
}
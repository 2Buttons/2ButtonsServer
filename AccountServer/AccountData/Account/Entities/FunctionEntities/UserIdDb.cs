using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountData.Account.Entities.FunctionEntities
{
  public class UserIdDb
  {
    [Key]
    [Column("id")]
    public int UserId { get; set; }
  }
}

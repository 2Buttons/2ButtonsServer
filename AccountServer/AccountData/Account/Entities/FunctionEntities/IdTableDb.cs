using System.ComponentModel.DataAnnotations.Schema;

namespace AccountData.Account.Entities.FunctionEntities
{
  [Table("idTable")]
  internal class IdTable
  {
    [Column("id")]
    public int VkId { get; set; }
  }
}
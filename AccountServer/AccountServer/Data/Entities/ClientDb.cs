using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountServer.Data.Entities
{
  [Table("Clients")]
    public class ClientDb
    {
      [Key]
      public int ClientId { get; set; }
      public string Secret { get; set; }
      public ApplicationType ApplicationType { get; set; }
      public bool IsActive { get; set; }
      public int RefreshTokenLifeTime { get; set; }
      public string AllowedOrigin { get; set; }
  }
}

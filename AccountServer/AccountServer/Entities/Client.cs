using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountServer.Models;

namespace AccountServer.Entities
{
  [Table("Clients")]
    public class Client
    {
      [Key]
      public int ClientId { get; set; }
      public string Secret { get; set; }
      public ApplicationTypes ApplicationType { get; set; }
      public bool IsActive { get; set; }
      public long RefreshTokenLifeTime { get; set; }
      public string AllowedOrigin { get; set; }
  }
}

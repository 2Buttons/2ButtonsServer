using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountServer.Data.Entities
{
  [Table("Tokens")]
  public class TokenDb
  {
    [Key]
    public int TokenId { get; set; }
    public int UserId { get; set; } // именно юзер
    public int ClientId { get; set; } // где юзе вошел или телефон или какой-то другой веб сайт короче для разных входов юзера
    public DateTime IssuedUtc { get; set; }
    public DateTime ExpiresUtc { get; set; }
    public string RefreshToken { get; set; }
  }
}

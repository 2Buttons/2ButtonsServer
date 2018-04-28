using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountServer.Models
{
    [Table("token")]
    public class Token
    {
        [Column("id")]
        [Key]
        public string Id { get; set; }

        [Column("client_id")]
        public string UserId { get; set; }

        [Column("refresh_token")]
        public string RefreshToken { get; set; }

        [Column("isStop")]
        public int IsStop { get; set; }
    }
}

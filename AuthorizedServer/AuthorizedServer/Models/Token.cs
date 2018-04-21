using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizedServer.Models
{
    [Table("rtoken")]
    public class Token
    {
        [Column("id")]
        [Key]
        public string Id { get; set; }

        [Column("client_id")]
        public string ClientId { get; set; }

        [Column("refresh_token")]
        public string RefreshToken { get; set; }

        [Column("isstop")]
        public int IsStop { get; set; }
    }
}

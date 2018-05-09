using System;

namespace AccountServer.Data.Entities
{
    public partial class Tokens
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string RefreshToken { get; set; }
    }
}

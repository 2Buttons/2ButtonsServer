namespace AccountServer.Data.Entities
{
    public partial class Clients
    {
        public int ClientId { get; set; }
        public string Secret { get; set; }
        public int? ApplicationType { get; set; }
        public bool? IsActive { get; set; }
        public int? RefreshTokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }
    }
}

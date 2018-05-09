namespace AccountServer.Data.Entities
{
    public partial class Users
    {
        public string UserId { get; set; }
        public int AccessFailedCount { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int RoleType { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int VkId { get; set; }
        public string VkToken { get; set; }
        public int FacebookId { get; set; }
        public string FacebookToken { get; set; }
    }
}

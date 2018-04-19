using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class FollowerDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public int? Age { get; set; }
        public int? Sex { get; set; }
        public bool Deleted { get; set; }
        public bool YouFollowed { get; set; }
        public bool HeFollowed { get; set; }
    }
}

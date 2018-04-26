using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class RecommendedStrangersDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
        public bool Followed { get; set; }
    }
}

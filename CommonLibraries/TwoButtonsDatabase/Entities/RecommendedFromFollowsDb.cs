using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class RecommendedFromFollowsDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string AvatarLink { get; set; }
        public int? Age { get; set; }
        public int? Sex { get; set; }
        public int? NetworkId { get; set; }
    }
}

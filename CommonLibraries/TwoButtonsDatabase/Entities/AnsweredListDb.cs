using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class AnsweredListDb
    {
        [Key]
        public int UserId { get; set; }
        public string Logingt { get; set; }
        public string SmallAvatarLink { get; set; }
        public int? Age { get; set; }
        public int? Sex { get; set; }
        public bool HeFollowed { get; set; }
        public bool YouFollowed { get; set; }
    }

}

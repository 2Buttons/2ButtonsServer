using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class AnsweredFollowToAndStrangersDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string AvatarLink { get; set; }
        public int? Age { get; set; }
        public int? Sex { get; set; }
        public bool HeFollowed { get; set; }
    }
}

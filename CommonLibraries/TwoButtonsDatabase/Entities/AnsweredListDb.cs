using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class AnsweredListDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
        public int HeFollowed { get; set; }
        public int YouFollowed { get; set; }
    }

}

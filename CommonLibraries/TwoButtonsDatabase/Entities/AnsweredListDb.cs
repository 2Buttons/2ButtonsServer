using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace TwoButtonsDatabase.Entities
{
    public partial class AnsweredListDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public int Age { get; set; }
        public SexType Sex { get; set; }
        public int HeFollowed { get; set; }
        public int YouFollowed { get; set; }
    }

}

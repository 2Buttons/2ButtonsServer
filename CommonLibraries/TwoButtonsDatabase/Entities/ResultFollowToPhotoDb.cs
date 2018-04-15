using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class ResultFollowToPhotoDb
    {
        [Key]
        public int UserId { get; set; }
        public string AvatarLink { get; set; }
    }

}

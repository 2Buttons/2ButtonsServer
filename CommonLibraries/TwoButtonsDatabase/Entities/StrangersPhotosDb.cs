using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class StrangersPhotosDb
    {
        [Key]
        public int UserId { get; set; }
        public string SmallAvatarLink { get; set; }
    }
}

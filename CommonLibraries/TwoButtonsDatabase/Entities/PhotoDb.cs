using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public class PhotoDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public int Sex { get; set; }
        public int Age { get; set; }
        public string SmallAvatarLink { get; set; }
        public string City { get; set; }
    }
}

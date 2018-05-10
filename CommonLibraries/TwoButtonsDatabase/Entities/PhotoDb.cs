using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace TwoButtonsDatabase.Entities
{
    public class PhotoDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public SexType Sex { get; set; }
        public int Age { get; set; }
        public string SmallAvatarLink { get; set; }
        public string City { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace SocialData.Main.Entities.Recommended
{
    public partial class RecommendedFromContactsDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string AvatarLink { get; set; }
        public DateTime BirthDate { get; set; }
        public int Sex { get; set; }
        public int NetworkId { get; set; }
    }
}

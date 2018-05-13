using System;
using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities.Recommended
{
    public partial class RecommendedStrangersDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public DateTime BirthDate { get; set; }
        public int Sex { get; set; }
        public bool Followed { get; set; }
    }
}

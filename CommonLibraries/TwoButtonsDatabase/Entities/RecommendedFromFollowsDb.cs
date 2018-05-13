using System;
using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class RecommendedFromFollowsDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public DateTime BirthDate { get; set; }
        public int Sex { get; set; }
        public int NetworkId { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace TwoButtonsDatabase.Entities.Recommended
{
    public partial class RecommendedFromFollowsDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public DateTime BirthDate { get; set; }
        public SexType Sex { get; set; }
        public int CommonFollowsTo { get; set; }
    }
}

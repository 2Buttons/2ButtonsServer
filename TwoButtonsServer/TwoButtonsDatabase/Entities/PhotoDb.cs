using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class PhotoDb
    {
        [Key]
        public int UserId { get; set; }
        public string AvatarLink { get; set; }
        public int? Anwser { get; set; }
    }
}

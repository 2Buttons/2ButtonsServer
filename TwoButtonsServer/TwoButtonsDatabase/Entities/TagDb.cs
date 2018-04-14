using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class TagDb
    {
        [Key]
        public int TagId { get; set; }
        public string TagText { get; set; }
        public int? Position { get; set; }
    }
}

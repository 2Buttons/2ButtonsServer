using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class ResultsDb
    {
        [Key]
        public int FirstOption { get; set; }
        public int SecondOption { get; set; }
        public int YourAnswer { get; set; }
    }
}

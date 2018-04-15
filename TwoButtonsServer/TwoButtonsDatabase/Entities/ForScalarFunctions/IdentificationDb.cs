using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities.ForScalarFunctions
{
    public class IdentificationDb
    {
        [Key]
        public int UserId { get; set; }
    }
}

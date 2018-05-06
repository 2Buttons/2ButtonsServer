using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities.Account
{
    public class UserContactsDb
    {
        [Key]
        public int NetworkId { get; set; }
        public string Account { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities.User
{
    public class UserContactsDb
    {
        [Key]
        public int NetworkId { get; set; }
        public string Account { get; set; }
    }
}
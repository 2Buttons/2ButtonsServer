using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities.Account
{
    public class RoleDb
    {
        [Key]
        public int Role { get; set; }
    }
}

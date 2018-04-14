using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TwoButtonsDatabase.Entities
{
    [Table("User")]
    public class UserDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public int Password { get; set; }
    }
}

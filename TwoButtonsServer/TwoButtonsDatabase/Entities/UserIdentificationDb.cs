using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TwoButtonsDatabase.Entities
{
    public partial class UserIdentificationDb
    {
        [Key]
        public int result { get; set; }
    }
}

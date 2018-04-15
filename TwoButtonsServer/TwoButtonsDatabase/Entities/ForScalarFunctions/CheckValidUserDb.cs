using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TwoButtonsDatabase.Entities.ForScalarFunctions
{
    public class CheckValidUserDb
    {
        [Key]
        public int ReturnCode { get; set; }
    }
}

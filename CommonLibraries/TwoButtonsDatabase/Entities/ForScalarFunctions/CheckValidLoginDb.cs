using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TwoButtonsDatabase.Entities.ForScalarFunctions
{
    public class CheckValidLoginDb
    {
        [Key]
        public int IsValid { get; set; }
    }
}

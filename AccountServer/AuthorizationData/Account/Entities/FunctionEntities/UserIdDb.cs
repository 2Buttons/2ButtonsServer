using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TwoButtonsAccountDatabase.Entities.FunctionEntities
{
  public class UserIdDb
  {
    [Key]
    [Column("id")]
    public int UserId { get; set; }
  }
}

﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountData.Main.Entities
{
  [Table("Cities")]
  public class CityEntity
  {
    [Key]
    public int CityId { get; set; }
    public string Name { get; set; }
    public int Inhabitants { get; set; }
  }
}
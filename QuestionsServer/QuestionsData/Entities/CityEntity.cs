using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsData.Entities
{
  [Table("City")]
  public class CityEntity
  {
    [Key]
    public int CityId { get; set; }

    public string Name { get; set; }
    public int People { get; set; }
  }
}
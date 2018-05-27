using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace SocialData.Account.Entities.FunctionEntities
{
  public class ExternalIdDb
  {
    [Key]
    public int ExternalId { get; set; }
  }
}
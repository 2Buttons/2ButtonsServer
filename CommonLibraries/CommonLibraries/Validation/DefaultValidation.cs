using System.ComponentModel.DataAnnotations;

namespace CommonLibraries.Validation
{
  public class IdValidationtAttribute : ValidationAttribute
  {
    public IdValidationtAttribute() : this("Id")
    {
    }

    public IdValidationtAttribute(string name)
    {
      ErrorMessage = $"{name} has to be more than 0.";
    }

    public override bool IsValid(object value)
    {
      return value is int && (int) value > 0;
    }
  }
}
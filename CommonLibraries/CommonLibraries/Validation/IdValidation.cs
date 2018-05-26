using System.ComponentModel.DataAnnotations;

namespace CommonLibraries.Validation
{
  public class IdValidationAttribute : ValidationAttribute
  {
    public IdValidationAttribute() : this("Id")
    {
    }

    public IdValidationAttribute(string name)
    {
      ErrorMessage = $"{name} has to be more than 0.";
    }

    public override bool IsValid(object value)
    {
      return value is int && (int) value > 0;
    }
  }
}
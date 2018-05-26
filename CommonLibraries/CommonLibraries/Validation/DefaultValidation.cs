using System.ComponentModel.DataAnnotations;

namespace CommonLibraries.Validation
{
  public class NotDefaultIntAttribute : ValidationAttribute
  {
    public NotDefaultIntAttribute() : this("Parameter")
    {
    }

    public NotDefaultIntAttribute(string name)
    {
      ErrorMessage = $"{name} is 0. It is not possible.";
    }

    public override bool IsValid(object value)
    {
      return value is int && (int) value != 0;
    }
  }
}
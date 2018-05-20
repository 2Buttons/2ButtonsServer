using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuthorizationServer.Helpers
{
    public static class Errors
    {
      public static ModelStateDictionary AddErrorToModelState(string code, string description, ModelStateDictionary modelState)
      {
        modelState.TryAddModelError(code, description);
        return modelState;
      }
  }
}

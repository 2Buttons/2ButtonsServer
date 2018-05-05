using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AccountServer.Helpers
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

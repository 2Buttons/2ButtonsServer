using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace CommonLibraries.Exceptions
{
    public static class ApplicationBuilderExstensions
  {
      public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
      {
        if (app == null)
          throw new ArgumentNullException(nameof(app));
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
      }
  }
}

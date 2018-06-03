using System;

namespace CommonLibraries.Exceptions.ApiExceptions
{
  public class NotFoundException : Exception
  {
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception e) : base(message, e) { }
  }
}
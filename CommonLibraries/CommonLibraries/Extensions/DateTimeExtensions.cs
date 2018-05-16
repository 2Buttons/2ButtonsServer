using System;

namespace CommonLibraries.Extensions
{
  public static class DateTimeExtensions
  {

    /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
    public static long ToUnixEpochDate(this DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);


    public static DateTime WhenBorned(this int age)
    {
      return DateTime.UtcNow.AddYears(-age);
    }

    /// <summary>
    /// Calculates the age in years of the current System.DateTime object today.
    /// </summary>
    /// <param name="birthDate">The date of birth</param>
    /// <returns>Age in years today. 0 is returned for a future date of birth.</returns>
    public static int Age(this DateTime birthDate)
    {
      return Age(birthDate, DateTime.Today);
    }

    /// <summary>
    /// Calculates the age in years of the current System.DateTime object on a later date.
    /// </summary>
    /// <param name="birthDate">The date of birth</param>
    /// <param name="laterDate">The date on which to calculate the age.</param>
    /// <returns>Age in years on a later day. 0 is returned as minimum.</returns>
    public static int Age(this DateTime birthDate, DateTime laterDate)
    {
      int age;
      age = laterDate.Year - birthDate.Year;

      if (age > 0)
      {
        age -= Convert.ToInt32(laterDate.Date < birthDate.Date.AddYears(age));
      }
      else
      {
        age = 0;
      }

      return age;
    }
  }
}

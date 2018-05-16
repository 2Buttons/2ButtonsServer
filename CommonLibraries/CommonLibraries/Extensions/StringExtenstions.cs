using System.Security.Cryptography;
using System.Text;

namespace CommonLibraries.Extensions
{
  public static class StringExtenstions
  {
    public static bool IsNullOrEmpty(this string str)
    {
      return string.IsNullOrEmpty(str);
    }

    public static string GetHashString(this string input, string format = null)
    {
      var md5 = MD5.Create();
      var inputBytes = Encoding.ASCII.GetBytes(input);
      var hash = md5.ComputeHash(inputBytes);
      var sb = new StringBuilder();
      foreach (var t in hash)
        sb.Append(t.ToString(format ?? "x2"));
      return sb.ToString();
    }
  }
}
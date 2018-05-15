using System.Security.Cryptography;
using System.Text;

namespace TwoButtonsAccountDatabase
{
  public static class Extenstions
  {
    public static string GetHashString(this string input)
    {
      var md5 = MD5.Create();
      var inputBytes = Encoding.ASCII.GetBytes(input);
      var hash = md5.ComputeHash(inputBytes);
      var sb = new StringBuilder();
      foreach (var t in hash)
        sb.Append(t.ToString("x2"));
      return sb.ToString();
    }
  }
}
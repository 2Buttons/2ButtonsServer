using System;
using System.Security.Cryptography;
using System.Text;

namespace CommonLibraries.Extensions
{
  public static class StringExtenstions
  {
    public static bool IsNullOrEmpty(this string str)
    {
      return String.IsNullOrEmpty(str);
    }

    public static string GetMd5HashString(this string input, string format = null)
    {
      var md5 = MD5.Create();
      // Преобразуем входную строку в массив байт и вычисляем хэш
      var inputBytes = Encoding.ASCII.GetBytes(input);
      var hash = md5.ComputeHash(inputBytes);
      var sb = new StringBuilder();
      // Преобразуем каждый байт хэша в шестнадцатеричную строку
      foreach (var t in hash)
        //указывает, что нужно преобразовать элемент в шестнадцатиричную строку длиной в два символа
        sb.Append(t.ToString(format ?? "x2"));
      return sb.ToString();
    }
  }
}
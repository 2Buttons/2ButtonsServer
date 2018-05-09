using System.Security.Cryptography;
using System.Text;

namespace TwoButtonsAccountDatabase
{
  public static class Extenstions
  {
    public static string GetHashString(this string s)
    {
      //переводим строку в байт-массим  
      var bytes = Encoding.Unicode.GetBytes(s);

      //создаем объект для получения средств шифрования  
      var CSP =
        new MD5CryptoServiceProvider();

      //вычисляем хеш-представление в байтах  
      var byteHash = CSP.ComputeHash(bytes);

      var hash = string.Empty;

      //формируем одну цельную строку из массива  
      foreach (var b in byteHash)
        hash += string.Format("{0:x2}", b);

      return hash;
    }
  }
}
using System.Security.Cryptography;
using System.Text;

namespace MediaServer.Infrastructure
{
    public static class StringHelper
    {
        public static string GetMd5Hash(this string input)
        {
            // создаем объект этого класса. Отмечу, что он создается не через new, а вызовом метода Create
            var md5Hasher = MD5.Create();

            // Преобразуем входную строку в массив байт и вычисляем хэш
            var data = md5Hasher.ComputeHash(Encoding.ASCII.GetBytes(input));

            // Создаем новый Stringbuilder (Изменяемую строку) для набора байт
            var sBuilder = new StringBuilder();

            // Преобразуем каждый байт хэша в шестнадцатеричную строку
            for (var i = 0; i < data.Length; i++)
                //указывает, что нужно преобразовать элемент в шестнадцатиричную строку длиной в два символа
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }
    }
}
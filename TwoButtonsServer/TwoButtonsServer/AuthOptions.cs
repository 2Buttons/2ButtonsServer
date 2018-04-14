using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TwoButtonsServer
{
    public class AuthOptions
    {
        public const string ISSUER = "TwoButtonsAuthServer"; // издатель токена
        public const string AUDIENCE = "http://*:6255/"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}

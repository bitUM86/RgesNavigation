using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RgesNaviApi
{
    public class AuthOptions
    {
        public const string ISSUER = "https://localhost:7155"; // издатель токена
        public const string AUDIENCE = "https://localhost:7155"; // потребитель токена
        const string KEY = "haligali-paratruper";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}

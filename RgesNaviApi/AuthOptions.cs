using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RgesNaviApi;

public static class AuthOptions
{
    public const string Issuer = "https://localhost:7155"; // издатель токена
    public const string Audience = "https://localhost:7155"; // потребитель токена
    const string Key = "haligali-paratruper";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
}
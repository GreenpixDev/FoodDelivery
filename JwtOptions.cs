using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FoodDelivery;

public class JwtOptions
{
    public const string Issuer = "MyAuthServer";
    public const string Audience = "MyAuthClient";

    private const string Key = "mysupersecret_secretkey!123";
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
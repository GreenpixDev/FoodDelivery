using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FoodDelivery;

public class JwtOptions
{
    public const string Issuer = "Delivery.Api";
    public const string Audience = "Delivery.Api";

    private const string Key = "maz6*TktI0J*6fmueLXwKUtsKau%Kyu3";
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
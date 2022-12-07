using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FoodDelivery.Configuration;

public class JwtConfiguration
{
    public const string Issuer = "Delivery.Api";
    public const string Audience = "Delivery.Api";
    public const int Lifetime = 60;
    
    private const string Key = "maz6*TktI0J*6fmueLXwKUtsKau%Kyu3";
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
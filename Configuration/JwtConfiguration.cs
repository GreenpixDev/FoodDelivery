using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FoodDelivery.Configuration;

public class JwtConfiguration
{
    public const string Issuer = "Delivery.Api";
    public const string Audience = "Delivery.Api";
    public const int Lifetime = 60;
    
    public const string NameIdClaim = "nameid";
    public const string NameClaim = "name";
    public const string EmailClaim = "email";
    
    private const string Key = "maz6*TktI0J*6fmueLXwKUtsKau%Kyu3";
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
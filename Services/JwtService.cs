using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FoodDelivery.Configuration;
using FoodDelivery.Models.Entity;
using Microsoft.IdentityModel.Tokens;

namespace FoodDelivery.Services;

public class JwtService : IJwtService
{
    private const string NameIdClaimType = "nameid";
    private const string NameClaimType = "name";
    private const string EmailClaimType = "email";
    
    public JwtSecurityToken GetToken(User user)
    {
        return new JwtSecurityToken(
            issuer: JwtConfiguration.Issuer,
            audience: JwtConfiguration.Audience,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(JwtConfiguration.Lifetime),
            claims: new []
            {
                new Claim(NameIdClaimType, user.Id.ToString()),
                new Claim(NameClaimType, user.Email),
                new Claim(EmailClaimType, user.Email)
            },
            signingCredentials: new SigningCredentials(
                JwtConfiguration.GetSymmetricSecurityKey(), 
                SecurityAlgorithms.HmacSha256
                )
            );
    }
}
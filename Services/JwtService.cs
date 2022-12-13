using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FoodDelivery.Configuration;
using FoodDelivery.Models.Entity;
using Microsoft.IdentityModel.Tokens;

namespace FoodDelivery.Services;

public class JwtService : IJwtService
{
    public JwtSecurityToken GetToken(User user)
    {
        return new JwtSecurityToken(
            issuer: JwtConfiguration.Issuer,
            audience: JwtConfiguration.Audience,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(JwtConfiguration.Lifetime),
            claims: new []
            {
                new Claim(JwtConfiguration.NameIdClaim, user.Id.ToString()),
                new Claim(JwtConfiguration.NameClaim, user.Email),
                new Claim(JwtConfiguration.EmailClaim, user.Email)
            },
            signingCredentials: new SigningCredentials(
                JwtConfiguration.GetSymmetricSecurityKey(), 
                SecurityAlgorithms.HmacSha256
                )
            );
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace FoodDelivery.Services;

public class JwtService : IJwtService
{
    public JwtSecurityToken GetToken()
    {
        return new JwtSecurityToken(
            issuer: JwtOptions.Issuer,
            audience: JwtOptions.Audience,
            expires: DateTime.Now.AddHours(3),
            claims: new []
            {
                new Claim("nameid", "kek"),
                new Claim("name", "ani"),
                new Claim("email", "ani")
            },
            signingCredentials: new SigningCredentials(
                JwtOptions.GetSymmetricSecurityKey(), 
                SecurityAlgorithms.HmacSha256
                )
            );
    }
}
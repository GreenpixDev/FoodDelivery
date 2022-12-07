using System.IdentityModel.Tokens.Jwt;
using FoodDelivery.Models.Entity;

namespace FoodDelivery.Services;

public interface IJwtService
{
    JwtSecurityToken GetToken(User user);
}
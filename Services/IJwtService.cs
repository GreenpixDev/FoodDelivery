using System.IdentityModel.Tokens.Jwt;

namespace FoodDelivery.Services;

public interface IJwtService
{
    JwtSecurityToken GetToken();
}
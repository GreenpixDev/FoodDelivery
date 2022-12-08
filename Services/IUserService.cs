using System.Security.Claims;
using FoodDelivery.Models.Dto;

namespace FoodDelivery.Services;

public interface IUserService
{
    TokenDto Register(UserRegisterDto userRegisterDto);
    
    TokenDto Login(LoginDto loginDto);
    
    void Logout(ClaimsPrincipal principal);

    UserDto GetProfile(ClaimsPrincipal principal);

    void UpdateProfile(ClaimsPrincipal principal, UserEditDto userEditDto);
}
using FoodDelivery.Models.Dto;

namespace FoodDelivery.Services;

public interface IUserService
{
    Task<TokenDto> Register(UserRegisterDto userRegisterDto);
    Task<TokenDto> Login(LoginDto loginDto);
    void Logout();
}
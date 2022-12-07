using FoodDelivery.Models.Dto;

namespace FoodDelivery.Services;

public interface IUserService
{
    TokenDto Register(UserRegisterDto userRegisterDto);
    TokenDto Login(LoginDto loginDto);
    void Logout();
}
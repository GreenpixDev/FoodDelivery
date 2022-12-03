using FoodDelivery.Database.Context;
using FoodDelivery.Models.Dto;
using FoodDelivery.Models.Entity;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Services;

public class UserService : IUserService
{

    private readonly FoodDeliveryContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserService(
        FoodDeliveryContext context, 
        UserManager<User> userManager, 
        SignInManager<User> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<TokenDto> Register(UserRegisterDto userRegisterDto)
    {
        User user = new User
        {
            UserName = userRegisterDto.Email,
            Email = userRegisterDto.Email,
            PhoneNumber = userRegisterDto.PhoneNumber,
            FullName = userRegisterDto.FullName,
            BirthDate = userRegisterDto.BirthDate,
            Gender = userRegisterDto.Gender,
            Address = userRegisterDto.Address
        };
        var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
        return new TokenDto
        {
            Token = result.Succeeded.ToString() // TODO сделать нормальную генерацию токена
        };
    }

    public async Task<TokenDto> Login(LoginDto loginDto)
    {
        var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, true, false);
        return new TokenDto
        {
            Token = result.Succeeded.ToString() // TODO сделать нормальную генерацию токена
        };
    }

    public void Logout()
    {
        throw new NotImplementedException();
    }
}
using System.IdentityModel.Tokens.Jwt;
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
    private readonly IJwtService _jwtService;

    public UserService(
        FoodDeliveryContext context, 
        UserManager<User> userManager, 
        SignInManager<User> signInManager,
        IJwtService jwtService)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
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
            Token = new JwtSecurityTokenHandler().WriteToken(_jwtService.GetToken())
        };
    }

    public async Task<TokenDto> Login(LoginDto loginDto)
    {
        var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, true, false);
        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(_jwtService.GetToken())
        };
    }

    public void Logout()
    {
        throw new NotImplementedException();
    }
}
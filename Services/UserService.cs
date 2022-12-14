using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FoodDelivery.Database.Context;
using FoodDelivery.Exception;
using FoodDelivery.Models.Dto;
using FoodDelivery.Models.Entity;
using FoodDelivery.Services.Password;
using FoodDelivery.Utils;

namespace FoodDelivery.Services;

public class UserService : IUserService
{

    private readonly FoodDeliveryContext _context;
    private readonly IPasswordEncoder _passwordEncoder;
    private readonly IJwtService _jwtService;

    public UserService(
        FoodDeliveryContext context,
        IPasswordEncoder passwordEncoder,
        IJwtService jwtService)
    {
        _context = context;
        _passwordEncoder = passwordEncoder;
        _jwtService = jwtService;
    }

    public TokenDto Register(UserRegisterDto userRegisterDto)
    {
        if (_context.Users.Any(user => user.Email == userRegisterDto.Email))
        {
            throw new DuplicateUserException();
        }
        
        User user = new User
        {
            Email = userRegisterDto.Email,
            PasswordHash = _passwordEncoder.Encode(userRegisterDto.Password),
            FullName = userRegisterDto.FullName,
            BirthDate = userRegisterDto.BirthDate,
            Gender = userRegisterDto.Gender,
            PhoneNumber = userRegisterDto.PhoneNumber,
            Address = userRegisterDto.Address
        };
        
        _context.Users.Add(user);
        _context.SaveChanges();
        
        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(_jwtService.GetToken(user))
        };
    }

    public TokenDto Login(LoginDto loginDto)
    {
        User? result = (
            from user in _context.Users
            where user.Email == loginDto.Email
            select user
        ).SingleOrDefault();

        if (result == null || !_passwordEncoder.Matches(result.PasswordHash, loginDto.Password))
        {
            throw new AuthenticationUserException();
        }
        
        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(_jwtService.GetToken(result))
        };
    }

    public void Logout(ClaimsPrincipal principal)
    {
        throw new NotImplementedException();
    }

    public UserDto GetProfile(ClaimsPrincipal principal)
    {
        User result = (
            from user in _context.Users
            where user.Id == ClaimsUtils.getId(principal)
            select user
        ).Single();

        return new UserDto
        {
            Id = result.Id,
            FullName = result.FullName,
            Email = result.Email,
            Address = result.Address,
            BirthDate = result.BirthDate,
            Gender = result.Gender,
            PhoneNumber = result.PhoneNumber
        };
    }

    public void UpdateProfile(ClaimsPrincipal principal, UserEditDto userEditDto)
    {
        User result = (
            from user in _context.Users
            where user.Id == ClaimsUtils.getId(principal)
            select user
        ).Single();

        result.FullName = userEditDto.FullName;
        result.BirthDate = userEditDto.BirthDate;
        result.Gender = userEditDto.Gender;
        result.Address = userEditDto.Address;
        result.PhoneNumber = userEditDto.PhoneNumber;

        _context.SaveChanges();
    }
}
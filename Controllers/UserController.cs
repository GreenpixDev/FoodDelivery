using FoodDelivery.Exception;
using FoodDelivery.Models.Dto;
using FoodDelivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[ApiController]
[Route("api/account")]
public class UserController : ControllerBase
{

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Зарегистрировать нового пользователя
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Register(UserRegisterDto userRegisterDto)
    {
        try
        {
            return new JsonResult(_userService.Register(userRegisterDto));
        }
        catch (DuplicateUserException)
        {
            return Conflict(new
            {
                DuplicateUserName = new []
                {
                    $"Username '{userRegisterDto.Email}' is already taken"
                }
            });
        }
    }
    
    /// <summary>
    /// Войти в систему
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login(LoginDto loginDto)
    {
        try
        {
            return new JsonResult(_userService.Login(loginDto));
        }
        catch (AuthenticationUserException)
        {
            return Unauthorized();
        }
    }
    
    /// <summary>
    /// Выйти из системы
    /// </summary>
    [HttpPost("logout"), Authorize]
    public void Logout()
    {
        _userService.Logout();
    }
    
    /// <summary>
    /// Получить профиль пользователя
    /// </summary>
    [HttpGet("profile"), Authorize]
    public UserDto GetProfile()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Изменить профиль пользователя
    /// </summary>
    [HttpPut("profile"), Authorize]
    public void UpdateProfile(UserEditDto userEditDto)
    {
        throw new NotImplementedException();
    }
}
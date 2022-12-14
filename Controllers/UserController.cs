using FoodDelivery.Exception;
using FoodDelivery.Models.Dto;
using FoodDelivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    public ActionResult<TokenDto> Register(UserRegisterDto userRegisterDto)
    {
        if ((userRegisterDto.BirthDate - DateTime.Now).TotalMilliseconds > 0)
        {
            return BadRequest(new
            {
                Message = "Birth date can't be later than today"
            });
        }
        try
        {
            return _userService.Register(userRegisterDto);
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
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public ActionResult<TokenDto> Login(LoginDto loginDto)
    {
        try
        {
            return _userService.Login(loginDto);
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
    public IActionResult Logout()
    {
        _userService.Logout(User, Request.Headers[HeaderNames.Authorization]
            .ToString()
            .Replace("Bearer ", "")
        );
        return Ok();
    }
    
    /// <summary>
    /// Получить профиль пользователя
    /// </summary>
    [HttpGet("profile"), Authorize]
    public ActionResult<UserDto> GetProfile()
    {
        return _userService.GetProfile(User);
    }
    
    /// <summary>
    /// Изменить профиль пользователя
    /// </summary>
    [HttpPut("profile"), Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public IActionResult UpdateProfile(UserEditDto userEditDto)
    {
        if ((userEditDto.BirthDate - DateTime.Now).TotalMilliseconds > 0)
        {
            return BadRequest(new
            {
                Message = "Birth date can't be later than today"
            });
        }
        _userService.UpdateProfile(User, userEditDto);
        return Ok();
    }
}
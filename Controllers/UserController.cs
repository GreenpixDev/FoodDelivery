using FoodDelivery.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[ApiController]
[Route("api/account")]
public class UserController : ControllerBase
{
    /// <summary>
    /// Зарегистрировать нового пользователя
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public TokenDto Register(UserRegisterDto userRegisterDto)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Войти в систему
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public TokenDto Login(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Выйти из системы
    /// </summary>
    [HttpPost("logout"), Authorize]
    public void Logout()
    {
        throw new NotImplementedException();
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
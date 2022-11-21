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
    public TokenDto Register(UserRegisterDto userRegisterDto)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Войти в систему
    /// </summary>
    [HttpPost("login")]
    public TokenDto Login()
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
    public void GetProfile()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Изменить профиль пользователя
    /// </summary>
    [HttpPut("profile"), Authorize]
    public void UpdateProfile()
    {
        throw new NotImplementedException();
    }
}
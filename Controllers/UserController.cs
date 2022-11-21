using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet("get")]
    public User Get()
    {
        _logger.Log(LogLevel.Information, "Test!");
        return new User
        {
            Email = "Test"
        };
    }
}
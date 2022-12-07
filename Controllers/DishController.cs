using FoodDelivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[ApiController]
[Route("api/dish")]
public class DishController : ControllerBase
{
    /// <summary>
    /// Получить список блюд (меню)
    /// </summary>
    [HttpGet]
    public IActionResult GetDishes(DishCategory[] categories, bool vegetarian, int page)
    {
        return Ok();
    }
    
    /// <summary>
    /// Получить информацию о конкретном блюде
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetDishById(string id)
    {
        return Ok();
    }
    
    /// <summary>
    /// Проверить, может ли пользователь установить рейтинг блюду
    /// </summary>
    [HttpGet("{id}/rating/check"), Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult CheckCanSetRating(string id)
    {
        return Ok();
    }
    
    /// <summary>
    /// Установить рейтинг для блюда
    /// </summary>
    [HttpPost("{id}/rating"), Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult SetRating(string id, int ratingScore)
    {
        return Ok();
    }
}
using FoodDelivery.Models;
using FoodDelivery.Models.Dto;
using FoodDelivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[ApiController]
[Route("api/dish")]
public class DishController : ControllerBase
{

    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    /// <summary>
    /// Получить список блюд (меню)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public ActionResult<DishPagesListDto> GetDishes(
        [FromQuery] List<DishCategory> categories,
        bool vegetarian = false,
        DishSorting? sorting = null,
        int page = 1)
    {
        return _dishService.GetDishPage(categories, vegetarian, sorting, page);
    }
    
    /// <summary>
    /// Получить информацию о конкретном блюде
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<DishDto> GetDishById(Guid id)
    {
        return _dishService.GetDishInfo(id);
    }
    
    /// <summary>
    /// Проверить, может ли пользователь установить рейтинг блюду
    /// </summary>
    [HttpGet("{id}/rating/check"), Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<bool> CheckCanSetRating(Guid id)
    {
        return _dishService.CanSetRating(User, id);
    }
    
    /// <summary>
    /// Установить рейтинг для блюда
    /// </summary>
    [HttpPost("{id}/rating"), Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public IActionResult SetRating(Guid id, int ratingScore)
    {
        _dishService.SetRating(User, id, ratingScore);
        return Ok();
    }
}
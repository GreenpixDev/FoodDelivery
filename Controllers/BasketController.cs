﻿using FoodDelivery.Exception;
using FoodDelivery.Models.Dto;
using FoodDelivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[ApiController]
[Route("api/basket")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    /// <summary>
    /// Получить корзину пользователя
    /// </summary>
    [HttpGet, Authorize]
    public ActionResult<List<DishBasketDto>> GetBasket()
    {
        return _basketService.GetBasket(User);
    }
    
    /// <summary>
    /// Добавить блюдо в корзину
    /// </summary>
    [HttpPost("dish/{dishId:guid}"), Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public IActionResult AddDish(Guid dishId)
    {
        try
        {
            _basketService.AddDishToBasket(User, dishId);
            return Ok();
        }
        catch (DishNotFoundException)
        {
            return NotFound(new
            {
                Message = $"Dish with id={dishId} don't in basket"
            });
        }
    }
    
    /// <summary>
    /// Уменьшает количество блюда в корзине (если increase = true), или удаляет блюдо из корзины полностью (если increase = false)
    /// </summary>
    [HttpDelete("dish/{dishId:guid}"), Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public IActionResult RemoveDish(Guid dishId, bool increase = false)
    {
        try
        {
            if (increase)
            {
                _basketService.DecreaseDishFromBasket(User, dishId);
            }
            else
            {
                _basketService.RemoveDishFromBasketCompletely(User, dishId);
            }
            return Ok();
        }
        catch (DishNotFoundException)
        {
            return NotFound(new
            {
                Message = $"Dish with id={dishId} don't in basket"
            });
        }
    }
}
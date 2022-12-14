using FoodDelivery.Exception;
using FoodDelivery.Models.Dto;
using FoodDelivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[ApiController]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Получить информацию о конкретном заказе
    /// </summary>
    [HttpGet("{id:guid}"), Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<OrderDto> GetOrderInfo(Guid id)
    {
        try
        {
            return _orderService.GetOrderInfo(User, id);
        }
        catch (OrderNotFoundException)
        {
            return NotFound(new
            {
                Message = $"Order with id={id} don't in database"
            });
        }
        catch (ForbiddenException)
        {
            return Forbid();
        }
    }
    
    /// <summary>
    /// Получить список заказов
    /// </summary>
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<List<OrderInfoDto>> GetOrderList()
    {
        return _orderService.GetOrderList(User);
    }
    
    /// <summary>
    /// Создать заказ на основе блюд из корзины пользователя
    /// </summary>
    [HttpPost, Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public IActionResult CreateOrderFromBasket(OrderCreateDto orderCreateDto)
    {
        if ((orderCreateDto.DeliveryTime - DateTime.Now).TotalMinutes < 60)
        {
            return BadRequest(new
            {
                Message = "Invalid delivery time. Delivery time must be more than current datetime on 60 minutes"
            });
        }
        try
        {
            _orderService.CreateOrderFromBasket(User, orderCreateDto);
            return Ok();
        }
        catch (EmptyBasketException e)
        {
            return BadRequest(new
            {
                Message = $"Empty basket for user with id={e.UserId}"
            });
        }
    }
    
    /// <summary>
    /// Подтвердить доставку заказа
    /// </summary>
    [HttpPost("{id:guid}/status"), Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public IActionResult ConfirmOrderDelivery(Guid id)
    {
        try
        {
            _orderService.ConfirmOrderDelivery(User, id);
            return Ok();
        }
        catch (OrderNotFoundException)
        {
            return NotFound(new
            {
                Message = $"Order with id={id} don't in database"
            });
        }
        catch (OrderConfirmedException)
        {
            return BadRequest(new
            {
                Message = $"Can't update status for order with id={id}"
            });
        }
        catch (ForbiddenException)
        {
            return Forbid();
        }
    }
}
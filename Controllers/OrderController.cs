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
        return _orderService.GetOrderInfo(User, id);
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
        _orderService.CreateOrderFromBasket(User, orderCreateDto);
        return Ok();
    }
    
    /// <summary>
    /// Подтвердить доставку заказа
    /// </summary>
    [HttpPost("{id:guid}/status"), Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public IActionResult ConfirmOrderDelivery(Guid id)
    {
        _orderService.ConfirmOrderDelivery(User, id);
        return Ok();
    }
}
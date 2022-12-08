using System.Security.Claims;
using FoodDelivery.Models.Dto;

namespace FoodDelivery.Services;

public interface IOrderService
{
    OrderDto GetOrderInfo(ClaimsPrincipal principal, Guid orderId);
    
    List<OrderInfoDto> GetOrderList(ClaimsPrincipal principal);

    void CreateOrderFromBasket(ClaimsPrincipal principal, OrderCreateDto orderCreateDto);

    void ConfirmOrderDelivery(ClaimsPrincipal principal, Guid orderId);
}
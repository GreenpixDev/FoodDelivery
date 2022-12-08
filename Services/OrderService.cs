using System.Security.Claims;
using FoodDelivery.Database.Context;
using FoodDelivery.Models.Dto;

namespace FoodDelivery.Services;

public class OrderService : IOrderService
{
    private readonly FoodDeliveryContext _context;

    public OrderService(FoodDeliveryContext context)
    {
        _context = context;
    }

    public OrderDto GetOrderInfo(ClaimsPrincipal principal, Guid orderId)
    {
        throw new NotImplementedException();
    }

    public List<OrderInfoDto> GetOrderList(ClaimsPrincipal principal)
    {
        throw new NotImplementedException();
    }

    public void CreateOrderFromBasket(ClaimsPrincipal principal, OrderCreateDto orderCreateDto)
    {
        throw new NotImplementedException();
    }

    public void ConfirmOrderDelivery(ClaimsPrincipal principal, Guid orderId)
    {
        throw new NotImplementedException();
    }
}
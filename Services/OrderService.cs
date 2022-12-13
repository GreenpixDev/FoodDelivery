using System.Security.Claims;
using FoodDelivery.Database.Context;
using FoodDelivery.Exception;
using FoodDelivery.Models;
using FoodDelivery.Models.Dto;
using FoodDelivery.Models.Entity;
using FoodDelivery.Utils;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
        var result = (
            from basketDish in _context.BasketDishes
            where basketDish.OrderId == orderId && basketDish.UserId == ClaimsUtils.getId(principal)
            select basketDish
        );

        throw new NotImplementedException(); // TODO
    }

    public List<OrderInfoDto> GetOrderList(ClaimsPrincipal principal)
    {
        var result = (
            from basketDish in _context.BasketDishes
            where basketDish.UserId == ClaimsUtils.getId(principal)
            select basketDish
        );
        
        throw new NotImplementedException(); // TODO
    }

    public void CreateOrderFromBasket(ClaimsPrincipal principal, OrderCreateDto orderCreateDto)
    {
        var result = (
            from basketDish in _context.BasketDishes
            where basketDish.OrderId == null && basketDish.UserId == ClaimsUtils.getId(principal)
            select basketDish
        );

        var price = result.Sum(basketDish => basketDish.Count * basketDish.Dish.Price);

        Order order = new Order
        {
            OrderTime = DateTime.Now,
            DeliveryTime = orderCreateDto.DeliveryTime,
            Address = orderCreateDto.Address,
            Price = price,
            Status = OrderStatus.InProcess
        };

        _context.Orders.Add(order);
        foreach (var basketDish in result)
        {
            basketDish.Order = order;
        }

        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            if (PostgresUtils.HasErrorCode(e, PostgresErrorCodes.UniqueViolation))
            {
                // TODO
            }
            
            if (PostgresUtils.HasErrorCode(e, PostgresErrorCodes.ForeignKeyViolation))
            {
                throw new NotFoundException();
            }

            throw;
        }
    }

    public void ConfirmOrderDelivery(ClaimsPrincipal principal, Guid orderId)
    {
        var order = new Order
        {
            Id = orderId
        };

        _context.Attach(order);
        order.Status = OrderStatus.Delivered;

        _context.SaveChanges();
    }
}
using System.Security.Claims;
using FoodDelivery.Database.Context;
using FoodDelivery.Exception;
using FoodDelivery.Models;
using FoodDelivery.Models.Dto;
using FoodDelivery.Models.Entity;
using FoodDelivery.Utils;
using Microsoft.EntityFrameworkCore;

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
        Order? order = _context.Orders
            .Where(order => order.Id == orderId)
            .Select(order => order)
            .Include(order => order.OrderDishes)
            .ThenInclude(dish => dish.Dish)
            .SingleOrDefault();

        if (order == null)
        {
            throw new OrderNotFoundException();
        }
        if (order.UserId != ClaimsUtils.getId(principal))
        {
            throw new ForbiddenException();
        }

        return new OrderDto
        {
            Id = order.Id,
            DeliveryTime = order.DeliveryTime,
            OrderTime = order.OrderTime,
            Address = order.Address,
            Price = order.Price,
            Status = order.Status,
            Dishes = (from dish in order.OrderDishes
                select new DishBasketDto
                {
                    Id = dish.Dish.Id,
                    Name = dish.Dish.Name,
                    Price = dish.Dish.Price,
                    TotalPrice = dish.Count * dish.Dish.Price,
                    Amount = dish.Count,
                    Image = dish.Dish.Image
                }).ToList()
        };
    }

    public List<OrderInfoDto> GetOrderList(ClaimsPrincipal principal)
    {
        List<Order> orders = _context.Orders
            .Where(order => order.UserId == ClaimsUtils.getId(principal))
            .Select(order => order)
            .ToList();

        return (from order in orders
            select new OrderInfoDto
            {
                Id = order.Id,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Status = order.Status,
                Price = order.Price
            }).ToList();
    }

    public void CreateOrderFromBasket(ClaimsPrincipal principal, OrderCreateDto orderCreateDto)
    {
        List<BasketDish> basketDishes = _context.BasketDishes
            .Where(basketDish => basketDish.UserId == ClaimsUtils.getId(principal))
            .Select(basketDish => basketDish)
            .Include(nameof(Dish))
            .ToList();

        if (!basketDishes.Any())
        {
            throw new EmptyBasketException { UserId = ClaimsUtils.getId(principal) };
        }
        
        double price = basketDishes.Sum(basketDish => basketDish.Count * basketDish.Dish.Price);
        Guid orderId = Guid.NewGuid();
        Order order = new Order
        {
            Id = orderId,
            UserId = ClaimsUtils.getId(principal),
            OrderTime = DateTime.UtcNow,
            DeliveryTime = orderCreateDto.DeliveryTime,
            Address = orderCreateDto.Address,
            Price = price,
            Status = OrderStatus.InProcess
        };
        _context.Orders.Add(order);
        _context.OrderDishes.AddRange(
            from basketDish in basketDishes
            select new OrderDish
            {
                DishId = basketDish.DishId,
                OrderId = orderId,
                Count = basketDish.Count
            }
        );
        _context.BasketDishes.RemoveRange(basketDishes);
        _context.SaveChanges();
    }

    public void ConfirmOrderDelivery(ClaimsPrincipal principal, Guid orderId)
    {
        Order? order = _context.Orders.Find(orderId);

        if (order == null)
        {
            throw new OrderNotFoundException();
        }
        
        if (order.UserId != ClaimsUtils.getId(principal))
        {
            throw new ForbiddenException();
        }
        
        if (order.Status == OrderStatus.Delivered)
        {
            throw new OrderConfirmedException();
        }
            
        order.Status = OrderStatus.Delivered;

        _context.Update(order);
        _context.SaveChanges();
    }
}
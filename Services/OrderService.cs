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
        List<OrderDish> orderDishes = _context.OrderDishes
            .Where(dish => dish.UserId == ClaimsUtils.getId(principal) && dish.OrderId == orderId)
            .Select(dish => dish)
            .Include(nameof(Order))
            .Include(nameof(Dish))
            .ToList();

        if (!orderDishes.Any())
        {
            // TODO not found
            throw new NotFoundException();
        }

        Order order = orderDishes.First().Order;

        return new OrderDto
        {
            Id = order.Id,
            DeliveryTime = order.DeliveryTime,
            OrderTime = order.OrderTime,
            Address = order.Address,
            Price = order.Price,
            Status = order.Status,
            Dishes = (from dish in orderDishes
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
        List<Order> orders = _context.OrderDishes
            .Where(dish => dish.UserId == ClaimsUtils.getId(principal))
            .Select(dish => dish.Order)
            .Distinct()
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
            // TODO пустая корзина
            throw new NotImplementedException();
        }
        
        double price = basketDishes.Sum(basketDish => basketDish.Count * basketDish.Dish.Price);
        Order order = new Order
        {
            Id = Guid.NewGuid(),
            OrderTime = DateTime.UtcNow,
            DeliveryTime = orderCreateDto.DeliveryTime,
            Address = orderCreateDto.Address,
            Price = price,
            Status = OrderStatus.InProcess
        };
        _context.Orders.Add(order);
        _context.BasketDishes.RemoveRange(basketDishes);
        _context.OrderDishes.AddRange(
            from basketDish in basketDishes
            select new OrderDish
            {
                DishId = basketDish.DishId,
                UserId = basketDish.UserId,
                OrderId = order.Id,
                Count = basketDish.Count
            }
        );
        _context.SaveChanges();
    }

    public void ConfirmOrderDelivery(ClaimsPrincipal principal, Guid orderId)
    {
        Order? order = _context.OrderDishes
            .Where(dish => dish.UserId == ClaimsUtils.getId(principal) && dish.OrderId == orderId)
            .Select(dish => dish.Order)
            .FirstOrDefault();

        if (order == null)
        {
            // TODO
            throw new NotFoundException();
        }

        if (order.Status == OrderStatus.Delivered)
        {
            // TODO
            throw new NotImplementedException();
        }
            
        order.Status = OrderStatus.Delivered;

        _context.Update(order);
        _context.SaveChanges();
    }
}
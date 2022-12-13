using System.Security.Claims;
using FoodDelivery.Database.Context;
using FoodDelivery.Exception;
using FoodDelivery.Models.Dto;
using FoodDelivery.Models.Entity;
using FoodDelivery.Utils;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FoodDelivery.Services;

public class BasketService : IBasketService
{
    private readonly FoodDeliveryContext _context;

    public BasketService(FoodDeliveryContext context)
    {
        _context = context;
    }

    public List<DishBasketDto> GetBasket(ClaimsPrincipal principal)
    {
        var result = (
            from basketDish in _context.BasketDishes
            where basketDish.OrderId == null && basketDish.UserId == ClaimsUtils.getId(principal)
            select basketDish
        );

        return (from basketDish in result
                select new DishBasketDto
                {
                    Id = basketDish.Dish.Id,
                    Name = basketDish.Dish.Name,
                    Image = basketDish.Dish.Image,
                    Price = basketDish.Dish.Price,
                    Amount = basketDish.Count,
                    TotalPrice = basketDish.Count * basketDish.Dish.Price
                }).ToList();
    }

    public void AddDishToBasket(ClaimsPrincipal principal, Guid dishId)
    {
        BasketDish basketDish = new BasketDish
        {
            UserId = ClaimsUtils.getId(principal),
            DishId = dishId,
            OrderId = null
        };
        
        _context.BasketDishes.Attach(basketDish);
        basketDish.Count++;
        
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

    public void DecreaseDishFromBasket(ClaimsPrincipal principal, Guid dishId)
    {
        BasketDish basketDish = new BasketDish
        {
            UserId = ClaimsUtils.getId(principal),
            DishId = dishId,
            OrderId = null
        };
        
        _context.BasketDishes.Attach(basketDish);
        basketDish.Count--;
        
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

    public void RemoveDishFromBasketCompletely(ClaimsPrincipal principal, Guid dishId)
    {
        BasketDish basketDish = new BasketDish
        {
            UserId = ClaimsUtils.getId(principal),
            DishId = dishId,
            OrderId = null
        };
        
        _context.BasketDishes.Remove(basketDish);
        
        _context.SaveChanges();
    }
}
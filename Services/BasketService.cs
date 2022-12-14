using System.Security.Claims;
using FoodDelivery.Database.Context;
using FoodDelivery.Exception;
using FoodDelivery.Models.Dto;
using FoodDelivery.Models.Entity;
using FoodDelivery.Utils;
using Microsoft.EntityFrameworkCore;

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
        List<BasketDish> basketDishes = _context.BasketDishes
            .Where(basketDish => basketDish.UserId == ClaimsUtils.getId(principal))
            .Select(basketDish => basketDish)
            .Include(nameof(Dish))
            .ToList();

        return (from basketDish in basketDishes
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
        if (!_context.Dishes.Any(dish => dish.Id == dishId))
        {
            throw new DishNotFoundException();
        }

        BasketDish? basketDish = _context.BasketDishes
            .Find(ClaimsUtils.getId(principal), dishId);

        if (basketDish == null)
        {
            basketDish = new BasketDish
            {
                UserId = ClaimsUtils.getId(principal),
                DishId = dishId,
                Count = 1
            };   
            _context.BasketDishes.Add(basketDish);
        }
        else
        {
            basketDish.Count++;
            _context.BasketDishes.Update(basketDish);
        }
        _context.SaveChanges();
    }

    public void DecreaseDishFromBasket(ClaimsPrincipal principal, Guid dishId)
    {
        BasketDish? basketDish = _context.BasketDishes
            .Find(ClaimsUtils.getId(principal), dishId);
        
        if (basketDish == null)
        {
            throw new DishNotFoundException();
        }
        
        basketDish.Count--;
        if (basketDish.Count == 0)
        {
            _context.BasketDishes.Remove(basketDish);
        }
        else
        {
            _context.BasketDishes.Update(basketDish);   
        }
        _context.SaveChanges();
    }

    public void RemoveDishFromBasketCompletely(ClaimsPrincipal principal, Guid dishId)
    {
        BasketDish? basketDish = _context.BasketDishes
            .Find(ClaimsUtils.getId(principal), dishId);
        
        if (basketDish == null)
        {
            throw new DishNotFoundException();
        }

        _context.BasketDishes.Remove(basketDish);
        _context.SaveChanges();
    }
}
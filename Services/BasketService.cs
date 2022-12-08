using System.Security.Claims;
using FoodDelivery.Database.Context;
using FoodDelivery.Models.Dto;

namespace FoodDelivery.Services;

public class BasketService : IBasketService
{
    private readonly FoodDeliveryContext _context;

    public BasketService(FoodDeliveryContext context)
    {
        _context = context;
    }

    public DishBasketDto GetBasket(ClaimsPrincipal principal)
    {
        throw new NotImplementedException();
    }

    public void AddDishToBasket(ClaimsPrincipal principal, Guid dishId)
    {
        throw new NotImplementedException();
    }

    public void DecreaseDishFromBasket(ClaimsPrincipal principal, Guid dishId)
    {
        throw new NotImplementedException();
    }

    public void RemoveDishFromBasketCompletely(ClaimsPrincipal principal, Guid dishId)
    {
        throw new NotImplementedException();
    }
}
using System.Security.Claims;
using FoodDelivery.Models.Dto;

namespace FoodDelivery.Services;

public interface IBasketService
{
    List<DishBasketDto> GetBasket(ClaimsPrincipal principal);

    void AddDishToBasket(ClaimsPrincipal principal, Guid dishId);
    
    void DecreaseDishFromBasket(ClaimsPrincipal principal, Guid dishId);
    
    void RemoveDishFromBasketCompletely(ClaimsPrincipal principal, Guid dishId);
}
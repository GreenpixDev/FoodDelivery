using System.Security.Claims;
using FoodDelivery.Models;
using FoodDelivery.Models.Dto;

namespace FoodDelivery.Services;

public interface IDishService
{
    DishPagesListDto GetDishPage(
        IList<DishCategory> categories, 
        bool vegetarian,
        DishSorting? sorting,
        int page);

    DishDto GetDishInfo(Guid dishId);

    bool CanSetRating(ClaimsPrincipal principal, Guid dishId);

    void SetRating(ClaimsPrincipal principal, Guid dishId, int rating);
}
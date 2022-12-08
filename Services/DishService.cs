using System.Security.Claims;
using FoodDelivery.Database.Context;
using FoodDelivery.Models;
using FoodDelivery.Models.Dto;

namespace FoodDelivery.Services;

public class DishService : IDishService
{
    private readonly FoodDeliveryContext _context;

    public DishService(FoodDeliveryContext context)
    {
        _context = context;
    }

    public DishPagesListDto GetDishPage(IList<DishCategory> categories, bool vegetarian, DishSorting? sorting, int page)
    {
        throw new NotImplementedException();
    }

    public DishDto GetDishInfo(Guid dishId)
    {
        throw new NotImplementedException();
    }

    public bool CanSetRating(ClaimsPrincipal principal, Guid dishId)
    {
        throw new NotImplementedException();
    }

    public void SetRating(ClaimsPrincipal principal, Guid dishId, int rating)
    {
        throw new NotImplementedException();
    }
}
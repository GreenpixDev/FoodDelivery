using FoodDelivery.Database.Context;

namespace FoodDelivery.Services;

public class DishService : IDishService
{
    private readonly FoodDeliveryContext _context;

    public DishService(FoodDeliveryContext context)
    {
        _context = context;
    }
}
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Database.Context;

public class FoodDeliveryContext : DbContext
{
    public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
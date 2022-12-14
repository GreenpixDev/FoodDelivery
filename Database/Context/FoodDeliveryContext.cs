using FoodDelivery.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Database.Context;

public class FoodDeliveryContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<DishRating> DishRatings { get; set; }
    public DbSet<BasketDish> BasketDishes { get; set; }
    
    public DbSet<OrderDish> OrderDishes { get; set; }

    public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
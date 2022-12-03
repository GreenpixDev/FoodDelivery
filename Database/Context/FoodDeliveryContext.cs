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

    public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BasketDish>()
            .HasIndex(x => new {x.UserId, x.DishId, x.OrderId})
            .IsUnique();
    }
}
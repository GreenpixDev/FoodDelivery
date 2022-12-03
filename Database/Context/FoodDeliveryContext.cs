﻿using FoodDelivery.Models.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Database.Context;

public class FoodDeliveryContext : IdentityDbContext<User>
{
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
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BasketDish>()
            .HasNoKey()
            .HasIndex(x => new {x.UserId, x.DishId, x.OrderId})
            .IsUnique();
    }
}
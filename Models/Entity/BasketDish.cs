﻿using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Models.Entity;

[PrimaryKey(nameof(UserId), nameof(DishId))]
public class BasketDish
{
    public Guid UserId { get; set; }
    
    public Guid DishId { get; set; }

    public User User { get; set; }
    
    public Dish Dish { get; set; }

    public int Count { get; set; }
}
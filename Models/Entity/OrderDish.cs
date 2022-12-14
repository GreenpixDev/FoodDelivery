using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Models.Entity;

[PrimaryKey(nameof(DishId), nameof(OrderId))]
public class OrderDish
{
    public Guid DishId { get; set; }
    
    public Guid OrderId { get; set; }

    public Dish Dish { get; set; }
    
    public Order Order { get; set; }

    public int Count { get; set; }
}
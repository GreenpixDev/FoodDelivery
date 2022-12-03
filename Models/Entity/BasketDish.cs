using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models.Entity;

public class BasketDish
{
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    
    [ForeignKey("Dish")]
    public Guid DishId { get; set; }
    
    [ForeignKey("Order")]
    public Guid OrderId { get; set; }

    public int Count { get; set; }
}
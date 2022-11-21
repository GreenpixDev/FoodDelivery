using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models.Entity;

public class BasketDish
{
    [Key]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    
    [Key]
    [ForeignKey("Dish")]
    public Guid DishId { get; set; }
    
    [Key]
    [ForeignKey("Order")]
    public Guid OrderId { get; set; }

    public int Count { get; set; }
}
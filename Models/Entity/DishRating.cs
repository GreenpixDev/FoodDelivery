using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models.Entity;

public class DishRating
{
    [Key]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    
    [Key]
    [ForeignKey("Dish")]
    public Guid DishId { get; set; }

    public double Rating { get; set; }
}
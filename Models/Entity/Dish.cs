using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.Entity;

public class Dish
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string Image { get; set; }
    
    public double Price { get; set; }
    
    public double Rating { get; set; }
    
    public bool Vegetarian { get; set; }
    
    public DishCategory Category { get; set; }
}
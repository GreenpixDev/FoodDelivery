using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.Dto;

public class DishBasketDto
{
    public Guid Id { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    public double TotalPrice { get; set; }
    
    [Required]
    public int Amount { get; set; }

    public string? Image { get; set; }
}
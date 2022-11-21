using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.Dto;

public class LoginDto
{
    [Required]
    [EmailAddress]
    [MinLength(1)]
    public string Email { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Password { get; set; }
}
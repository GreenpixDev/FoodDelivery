using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.Dto;

public class UserRegisterDto
{
    [Required]
    [MinLength(1)]
    public string FullName { get; set; }
    
    [Required]
    [MinLength(6)]
    
    [RegularExpression(@".*\d.*", ErrorMessage = "Password requires at least one digit")]
    public string Password { get; set; }
    
    [Required]
    [EmailAddress]
    [MinLength(1)]
    public string Email { get; set; }
    
    public string? Address { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
}
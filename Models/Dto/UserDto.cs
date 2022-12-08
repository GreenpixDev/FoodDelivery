using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.Dto;

public class UserDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [MinLength(1)]
    public string FullName { get; set; }
    
    public string Email { get; set; }

    public string? Address { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
}
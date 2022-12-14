using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Models.Entity;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public Guid Id { get; set; }
    
    public string Email { get; set; }
    
    public string PasswordHash { get; set; }

    public string FullName { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public Gender Gender { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? Address { get; set; }

}
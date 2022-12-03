using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Models.Entity;

public class User : IdentityUser
{
    [Key]
    public Guid Id { get; set; }

    public string FullName { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public Gender Gender { get; set; }
    
    public string? Address { get; set; }
    
}
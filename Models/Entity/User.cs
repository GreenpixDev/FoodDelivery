using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.Entity;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    public string Email { get; set; }
    
    public string FullName { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public Gender Gender { get; set; }
    
    public string? Address { get; set; }
    
    public string? Phone { get; set; }
}
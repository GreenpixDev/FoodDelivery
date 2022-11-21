using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.Dto;

public class TokenDto
{
    [Required]
    public string Token { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.Entity;

public class Order
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }

    public User User { get; set; }

    public List<OrderDish> OrderDishes { get; set; }

    public DateTime DeliveryTime { get; set; }
    
    public DateTime OrderTime { get; set; }
    
    public OrderStatus Status { get; set; }
    
    public double Price { get; set; }

    public string Address { get; set; }
}
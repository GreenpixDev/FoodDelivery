namespace FoodDelivery.Exception;

public class EmptyBasketException : System.Exception
{
    public Guid UserId { get; set; }
}
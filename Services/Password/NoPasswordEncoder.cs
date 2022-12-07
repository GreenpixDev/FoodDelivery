namespace FoodDelivery.Services.Password;

public class NoPasswordEncoder : IPasswordEncoder
{
    public string Encode(string rawPassword)
    {
        return rawPassword;
    }

    public bool Matches(string passwordHash, string rawPassword)
    {
        return passwordHash == rawPassword;
    }
}
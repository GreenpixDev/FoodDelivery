namespace FoodDelivery.Services.Password;

public class BCryptPasswordEncoder : IPasswordEncoder
{
    private const int Round = 10;
    
    public string Encode(string rawPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(rawPassword, Round);
    }

    public bool Matches(string passwordHash, string rawPassword)
    {
        return BCrypt.Net.BCrypt.Verify(rawPassword, passwordHash);
    }
}
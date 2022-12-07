namespace FoodDelivery.Services.Password;

public interface IPasswordEncoder
{
    string Encode(string rawPassword);

    bool Matches(string passwordHash, string rawPassword);
}
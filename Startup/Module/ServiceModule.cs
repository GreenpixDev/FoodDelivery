using FoodDelivery.Services;
using FoodDelivery.Services.Password;

namespace FoodDelivery.Startup.Module;

public class ServiceModule
{
    public static void AddDependencies(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDishService, DishService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordEncoder, BCryptPasswordEncoder>();
    }
}
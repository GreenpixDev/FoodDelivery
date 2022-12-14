using Microsoft.AspNetCore.Authorization;

namespace FoodDelivery.Startup.Module;

public class AppModule
{
    public static void AddDependencies(IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler, BlacklistTokenHandler>();
        services.AddHttpContextAccessor();
    }
}
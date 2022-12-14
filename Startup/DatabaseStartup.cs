using FoodDelivery.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Startup;

public class DatabaseStartup
{
    public static void UsePostgres(WebApplicationBuilder builder)
    {
        var connection = builder.Configuration.GetConnectionString("Postgres");
        builder.Services.AddDbContext<FoodDeliveryContext>(options => options.UseNpgsql(connection));
    }

    public static void UseRedis(WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
            options.InstanceName = "token";
        });
    }
}
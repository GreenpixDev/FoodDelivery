using System.Net.Mime;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Startup;

public class WebStartup
{
    public static void AddControllers(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    }

    public static void AddMvc(WebApplicationBuilder builder)
    {
        builder.Services.AddMvc(options =>
        {
            options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status200OK));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status500InternalServerError));
        });
    }

    public static void AddEndpointsApiExplorer(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
    }
}
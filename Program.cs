using System.Net.Mime;
using System.Reflection;
using System.Text.Json.Serialization;
using FoodDelivery.Configuration;
using FoodDelivery.Database.Context;
using FoodDelivery.Services;
using FoodDelivery.Services.Password;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = JwtConfiguration.Issuer,
        ValidateAudience = true,
        ValidAudience = JwtConfiguration.Audience,
        ValidateLifetime = true,
        IssuerSigningKey = JwtConfiguration.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddMvc(options =>
{
    options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
    options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status200OK));
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status500InternalServerError));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    // Swagger документация по xml
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    
    // Swagger авторизация
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Пожалуйста, введите токен",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Добавление базы данных
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FoodDeliveryContext>(options => options.UseNpgsql(connection));

// Настройка правил регистрации
/*builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<FoodDeliveryContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});*/

// Объявление зависимостей
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordEncoder, BCryptPasswordEncoder>();

// Приложение
var app = builder.Build();

// Для разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
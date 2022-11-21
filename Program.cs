using FoodDelivery.Database.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Добавление базы данных
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FoodDeliveryContext>(options => options.UseNpgsql(connection));

// Объявление зависимостей
// TODO

// Приложение
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); TODO добавить ssl сертификат

app.UseAuthorization();

app.MapControllers();

app.Run();
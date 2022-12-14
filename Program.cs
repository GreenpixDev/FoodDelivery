using FoodDelivery.Startup;
using FoodDelivery.Startup.Module;

var builder = WebApplication.CreateBuilder(args);

// Настройка приложения
AuthStartup.AddAuthentication(builder);
AuthStartup.AddAuthorization(builder);

WebStartup.AddControllers(builder);
WebStartup.AddMvc(builder);
WebStartup.AddEndpointsApiExplorer(builder);

SwaggerStartup.AddSwagger(builder);

// Подключения баз данных
DatabaseStartup.UsePostgres(builder);
DatabaseStartup.UseRedis(builder);

// Объявление зависимостей
AppModule.AddDependencies(builder.Services);
ServiceModule.AddDependencies(builder.Services);

// Приложение
var app = builder.Build();

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
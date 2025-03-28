using Microsoft.EntityFrameworkCore;
using VkBot.Models;
using VkBot.Services;
using VkNet;
using VkNet.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Добавляем конфигурацию
builder.Configuration.AddJsonFile("appsettings.json");

// Регистрируем сервисы
builder.Services.AddControllers();

// Добавляем кэширование в памяти
builder.Services.AddMemoryCache();

// Регистрируем VkApi
builder.Services.AddSingleton<VkApi>(provider =>
{
    var api = new VkApi();
    api.Authorize(new ApiAuthParams
    {
        AccessToken = builder.Configuration["VkSettings:AccessToken"]
    });
    return api;
});

// Регистрируем DbContext (EF Core)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрируем кастомные сервисы
builder.Services.AddScoped<TemplateService>();
builder.Services.AddScoped<BotService>();

// Настройка OpenAPI
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

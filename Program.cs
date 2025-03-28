using Microsoft.EntityFrameworkCore;
using VkBot.Models;
using VkBot.Services;
using VkNet;
using VkNet.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// ��������� ������������
builder.Configuration.AddJsonFile("appsettings.json");

// ������������ �������
builder.Services.AddControllers();

// ��������� ����������� � ������
builder.Services.AddMemoryCache();

// ������������ VkApi
builder.Services.AddSingleton<VkApi>(provider =>
{
    var api = new VkApi();
    api.Authorize(new ApiAuthParams
    {
        AccessToken = builder.Configuration["VkSettings:AccessToken"]
    });
    return api;
});

// ������������ DbContext (EF Core)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ������������ ��������� �������
builder.Services.AddScoped<TemplateService>();
builder.Services.AddScoped<BotService>();

// ��������� OpenAPI
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

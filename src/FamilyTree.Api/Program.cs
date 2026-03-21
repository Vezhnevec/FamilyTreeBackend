using FamilyTree.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔹 1. Добавляем контроллеры
builder.Services.AddControllers();

// 🔹 2. Добавляем Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Family Tree API",
        Version = "v1",
        Description = "API для управления семейным древом"
    });
});

// 🔹 3. РЕГИСТРИРУЕМ DbContext (это то, чего не хватало!)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 🔹 4. Настраиваем пайплайн запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Family Tree API v1");
        c.RoutePrefix = string.Empty; // Swagger открывается на корне
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
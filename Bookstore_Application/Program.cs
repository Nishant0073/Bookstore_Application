using System.Text.Json.Serialization;
using Bookstore_Application.Data;
using Bookstore_Application.Models;
using Bookstore_Application.Repositories;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                          throw new InvalidOperationException();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

//Adding Loggers for Repository
builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IRepository<Book>,BookRepository>();
builder.Services.AddScoped<IRepository<Order>,OrderRepository>();
builder.Services.AddScoped<IRepository<OrderItem>,OrderItemRepository>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Logging.ClearProviders(); // Clear default providers
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug); // Ensure minimum level is Debug

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
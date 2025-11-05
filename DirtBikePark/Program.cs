using Microsoft.EntityFrameworkCore;

using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IParkService, ParkService>();

// Add Swagger services ({protocol}://{urlBase}/swagger).
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework Core in-memory database.
// TODO Tyler

// Build the app.
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure controllers and run.
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
using Microsoft.EntityFrameworkCore;

using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IParkService, ParkService>();
builder.Services.AddScoped<IParkRepository, ParkRepository>();

// Add Swagger services ({protocol}://{urlBase}/swagger).
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework Core in-memory database.
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseInMemoryDatabase("DirtBikeDB");
});

// Build the app.
WebApplication app = builder.Build();

DataSeeder.Seed(app);

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
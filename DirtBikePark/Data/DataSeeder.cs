using DirtBikePark.Models;
using Microsoft.EntityFrameworkCore;

namespace DirtBikePark.Data
{
    public static class DataSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                // Change once switch to real SQL database
                context.Database.EnsureCreated();

                if (!context.Parks.Any())
                {
                    var parks = new List<Park> 
                    {
                        new Park { Id = 1, Name = "Park One", Description = "There are a lot of trees.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m },
                        new Park { Id = 2, Name = "Park Two", Description = "There is a river through the middle.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m  },
                        new Park { Id = 3, Name = "Park Three", Description = "It's pretty green.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m  }
                    };

                    context.Parks.AddRange(parks);
                    context.SaveChanges();
                }

                if (!context.Carts.Any())
                {
                    var carts = new List<Cart>
                    {
                        new Cart {Id = Guid.NewGuid() }
                    };
                    context.Carts.AddRange(carts);
                    context.SaveChanges();
                }

                if (!context.Bookings.Any())
                {
                    var bookings = new List<Booking>
                    {
                        new Booking {Id = 10, CartId = context.Carts.First().Id, ParkId = 3, Date = new DateOnly(2025, 12, 12), NumAdults = 2, NumChildren = 1, TotalPrice = 40.00m},
                        new Booking {Id = 11, CartId = context.Carts.First().Id, ParkId = 1, Date = new DateOnly(2025, 12, 10), NumAdults = 3, NumChildren = 0, TotalPrice = 45.00m}
                    };
                    context.Bookings.AddRange(bookings);
                    context.SaveChanges();
                }


            }
        }
    }
}

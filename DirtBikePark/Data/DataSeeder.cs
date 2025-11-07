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
            }
        }
    }
}

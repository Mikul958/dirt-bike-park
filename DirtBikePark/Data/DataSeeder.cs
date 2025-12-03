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
                //context.Database.EnsureCreated();

                context.Database.Migrate(); // This applies pending migrations
                // Once these are added run the following commands in the command line:
                // dotnet ef migrations add InitialCreate
                // dotnet ef database update

                if (!context.Parks.Any())
                {
                    var parks = new List<Park> 
                    {
                        new Park {     /* Id = 1,*/
                                        Name = "Grand Canyon Adventure Tour",
                                        Location = "Arizona, USA",
                                        Description = "A breathtaking tour of the Grand Canyon South Rim, including scenic viewpoints and a guided nature walk.",
                                        PricePerAdult = 150.75m,
                                        PricePerChild = 75.25m,
                                        GuestLimit = 20 
                        },
                        new Park {     /* Id = 2,*/
                                        Name = "Eiffel Tower Guided Visit",
                                        Location = "Paris, France",
                                        Description = "Skip the line and enjoy a guided tour to the top of the iconic Eiffel Tower with panoramic city views.",
                                        PricePerAdult = 45.00m,
                                        PricePerChild = 20.00m,
                                        GuestLimit = 15
                        },
                        new Park {      /*Id = 3,*/
                                        Name = "Kyoto Bamboo Grove Walk",
                                        Location = "Kyoto, Japan",
                                        Description = "A serene morning walk through the famous Arashiyama Bamboo Grove, followed by a traditional tea ceremony.",
                                        PricePerAdult = 80.50m,
                                        PricePerChild = 40.25m,
                                        GuestLimit = 10
                        }
                    };

                    context.Parks.AddRange(parks);
                    context.SaveChanges();
                }

                //if (!context.Carts.Any())
                //{
                //    var carts = new List<Cart>
                //    {
                //        new Cart {Id = Guid.NewGuid() }
                //    };
                //    context.Carts.AddRange(carts);
                //    context.SaveChanges();
                //}

                if (!context.Bookings.Any())
                {
                    var bookings = new List<Booking>
                    {
                        new Booking {/*Id = 10,*/ CartId = null, ParkId = 3, Date = new DateOnly(2025, 12, 12), NumAdults = 2, NumChildren = 1, TotalPrice = 40.00m},
                        new Booking {/*Id = 11,*/ CartId = null, ParkId = 1, Date = new DateOnly(2025, 12, 10), NumAdults = 3, NumChildren = 0, TotalPrice = 45.00m}
                    };
                    context.Bookings.AddRange(bookings);
                    context.SaveChanges();
                }


            }
        }
    }
}

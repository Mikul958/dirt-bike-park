using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using DirtBikePark.Data;
using DirtBikePark.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using Xunit;
using System.Linq;

namespace Tests
{

    public class DbTests
    {
        [Fact]
        public void Can_Add_Model_To_InMemory_Database()
        {
            // ARRANGE: Set up options for the in-memory database
            // We use a unique name (Guid) to ensure this database is isolated
            // from other tests that might be running.
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;

            // ACT: Save the data using a fresh context instance
            // The 'using' block ensures the context is disposed, but the *database* persists
            // because it's tied to the unique 'options' generated above.
            using (var context = new DatabaseContext(options))
            {
                var park = new Park { Id = 1, Name = "South Carolina Park", GuestLimit = 10, PricePerAdult = 5.00m, PricePerChild = 2.00m };
                context.Parks.Add(park);
                context.SaveChanges();
            }

            using (var context = new DatabaseContext(options))
            {
                // Check that exactly one park exists
                Assert.Equal(1, context.Parks.Count());

                // Check that we can retrive the specific park we added
                var retrievedPark = context.Parks.SingleOrDefault(x => x.Id == 1);
                Assert.NotNull(retrievedPark);
                Assert.Equal("South Carolina Park", retrievedPark.Name);

            }
        }
        
        [Fact]
        public void Can_Delete_Models_From_Database()
        {
            // ARRANGE: Set up options for the in-memory database
            // We use a unique name (Guid) to ensure this database is isolated
            // from other tests that might be running.
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;

            // ACT: Save the data using a fresh context instance
            // The 'using' block ensures the context is disposed, but the *database* persists
            // because it's tied to the unique 'options' generated above.
            using (var context = new DatabaseContext(options))
            {
                var park = new Park { Id = 1, Name = "South Carolina Park", GuestLimit = 10, PricePerAdult = 5.00m, PricePerChild = 2.00m };
                context.Parks.Add(park);
                context.SaveChanges();
                context.Remove(park);
                context.SaveChanges();
            }

            using (var context = new DatabaseContext(options))
            {
                Assert.Equal(0, context.Parks.Count());
            }
        }

        [Fact]
        public void Can_Link_Tables()
        {
            // ARRANGE: Set up options for the in-memory database
            // We use a unique name (Guid) to ensure this database is isolated
            // from other tests that might be running.
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;

            Park park = new Park { Id = 1, Name = "South Carolina Park", GuestLimit = 10, PricePerAdult = 5.00m, PricePerChild = 2.00m };
            Booking booking = new Booking { Id = 1, CartId = Guid.NewGuid(), ParkId = 1, NumDays = 3, NumAdults = 10, NumChildren = 10, TotalPrice = 10.00m };

            // ACT: Save the data using a fresh context instance
            // The 'using' block ensures the context is disposed, but the *database* persists
            // because it's tied to the unique 'options' generated above.
            using (var context = new DatabaseContext(options))
            {
                context.Parks.Add(park);
                context.Bookings.Add(booking);
                context.SaveChanges();

                Park? retrievedPark = context.Parks.Where(park => park.Id == 1).FirstOrDefault();
                Booking? retrievedBooking = context.Bookings.Where(booking => booking.Id == 1).FirstOrDefault();
                Park? retrievedParkThroughBooking = null;
                if (retrievedBooking != null)
                    retrievedParkThroughBooking = retrievedBooking.Park;

                Assert.Equal(retrievedPark, retrievedParkThroughBooking);
            }
        }
    }
}


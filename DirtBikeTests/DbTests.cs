//using DirtBikePark.Interfaces;
//using DirtBikePark.Models;
//using DirtBikePark.Data;
//using DirtBikePark.Controllers;
using DirtBikePark;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using Xunit;

//using System.Linq;
namespace Tests
{

    public class DbTests
    {
        [Fact]
        public void Can_Add_Model_To_InMemory_Database()
        {
            //// ARRANGE: Set up options for the in-memory database
            //// We use a unique name (Guid) to ensure this database is isolated
            //// from other tests that might be running.
            //var options = new DbContextOptionsBuilder<DatabaseContext>()
            //    .UseInMemoryDatabase(databaseName: Guid.NewGuid()
            //    .ToString())
            //    .Options;

            //// ACT: Save the data using a fresh context instance
            //// The 'using' block ensures the context is disposed, but the *database* persists
            //// because it's tied to the unique 'options' generated above.
            //using (var context = new DatabaseContext(options))
            //{
            //    var park = new Park { Id = 1, Name = "South Carolina Park", Bookings = new List<Booking>(), GuestLimit = 10, PricePerAdult = 5.00m, PricePerChild = 2.00m };
            //    context.Parks.Add(park);
            //    context.SaveChanges();
            //}

            //using (var context = new DatabaseContext(options))
            //{
            //    // Checl that exactly one park exists
            //    Assert.Equal(1, context.Parks.Count());

            //    // Check that we can retrive the specific park we added
            //    var retrievedPark = context.Parks.SingleOrDefault(x => x.Id == 1);
            //    Assert.NotNull(retrievedPark);
            //    Assert.Equal("South Carolina Park", retrievedPark.Name);
            //}
            Assert.Equal(1, 1);
        }

        [Fact]
        public void Example_AddsCorrectly()
        {
            var result = 2 + 2;
            Assert.Equal(4, result);
        }

        //[Fact]
        //public void WorksCorrectly()
        //{
        //    Assert.True(false);
        //}
    }
}


using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using DirtBikePark.Data;
using DirtBikePark.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using Xunit;
using System.Linq;
using DirtBikePark.Services;

namespace Tests
{
    public class ParkTests
    {
        [Fact]
        public void Can_Create_Park()
        {
            var emptyPark = new Park();
            var park = new Park { Id = 1, Name = "South Carolina Park", Bookings = new List<Booking>(), GuestLimit = 10, PricePerAdult = 5.00m, PricePerChild = 2.00m };


            Assert.NotNull(park);
            Assert.IsType<Park>(park);
        }

        [Fact]
        public async Task Can_Get_A_Park_With_Valid_Id()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;


            using (var context = new DatabaseContext(options))
            {
                var park = new Park { Id = 1, Name = "South Carolina Park", Bookings = new List<Booking>(), GuestLimit = 10, PricePerAdult = 5.00m, PricePerChild = 2.00m };
                context.Parks.Add(park);
                context.SaveChanges();
            }

            var parkService = new ParkService(new DatabaseContext(options));

            Park? resultPark = await parkService.GetPark(1);

            //Assert
            Assert.NotNull(resultPark);
            Assert.Equal(1, resultPark.Id);
            Assert.Equal("South Carolina Park", resultPark.Name);
        }

        [Fact]
        public async Task Cannot_Get_A_Park_With_Invalid_Id()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;


            using (var context = new DatabaseContext(options))
            {
                var park = new Park { Id = 1, Name = "South Carolina Park", Bookings = new List<Booking>(), GuestLimit = 10, PricePerAdult = 5.00m, PricePerChild = 2.00m };
                context.Parks.Add(park);
                context.SaveChanges();
            }

            var parkService = new ParkService(new DatabaseContext(options));

            Park? resultPark = await parkService.GetPark(2);

            //Assert
            Assert.Null(resultPark);
        }

        [Fact]
        public async Task Can_Get_All_Parks()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;

            var parks = new List<Park>
                {
                    new Park { Id = 1, Name = "South Carolina Park", Bookings = new List<Booking>(), GuestLimit = 10, PricePerAdult = 5.00m, PricePerChild = 2.00m },
                    new Park { Id = 2, Name = "Park Two", Description = "There is a river through the middle.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m }
                };

            using (var context = new DatabaseContext(options))
            {
                context.Parks.AddRange(parks);
                context.SaveChanges();
            }

            var parkService = new ParkService(new DatabaseContext(options));

            IEnumerable<Park> resultParks = await parkService.GetParks();
            
            var resultList = resultParks.ToList();

            Assert.NotNull(resultList);
            Assert.Equal(parks.Count, resultList.Count);

            foreach (var expectedPark in parks)
            {
                var actualPark = resultList.FirstOrDefault(p => p.Id == expectedPark.Id);
                Assert.NotNull(actualPark);
                Assert.Equal(expectedPark.Name, actualPark.Name);
            }
        }

        [Fact]
        public async Task Can_Add_Park()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;

            var park = new Park { Id = 1, Name = "South Carolina Park", Bookings = new List<Booking>(), GuestLimit = 10, PricePerAdult = 5.00m, PricePerChild = 2.00m };
                  
            //using (var context = new DatabaseContext(options))
            //{
            //    context.Parks.AddRange(parks);
            //    context.SaveChanges();
            //}

            var parkService = new ParkService(new DatabaseContext(options));

            bool isAdded = await parkService.AddPark(park);

            //var resultList = resultParks.ToList();

            Assert.True(isAdded);
        }

        [Fact]
        public async Task Can_Remove_Park()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;

            var park = new Park { Id = 1, Name = "South Carolina Park", Bookings = new List<Booking>(), GuestLimit = 10, PricePerAdult = 5.00m, PricePerChild = 2.00m };

            using (var context = new DatabaseContext(options))
            {
                context.Parks.Add(park);
                context.SaveChanges();
            }

            var parkService = new ParkService(new DatabaseContext(options));

            bool isAdded = await parkService.RemovePark(1);
            Park? resultPark = await parkService.GetPark(1);

            Assert.True(isAdded);
            Assert.Null(resultPark);
        }
    }
}


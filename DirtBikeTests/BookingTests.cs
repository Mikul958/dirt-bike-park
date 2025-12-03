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
    public class BookingTests
    {
        [Fact]
        public void Can_Create_Booking()
        {
            var emptyBooking = new Booking();
            var booking = new Booking { Id = 10, CartId = null, ParkId = 3, Date = new DateOnly(2025, 12, 12), NumAdults = 2, NumChildren = 1, TotalPrice = 40.00m };


            Assert.NotNull(booking);
            Assert.IsType<Booking>(booking);
            Assert.NotEqual(emptyBooking, booking);
        }

        [Fact]
        public async Task Can_Get_A_Booking_With_Valid_ParkId()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;


            using (var context = new DatabaseContext(options))
            {
                var park = new Park { Id = 3, Name = "Test Park" };
                context.Parks.Add(park);
                var booking = new Booking { Id = 10, CartId = null, ParkId = 3, Date = new DateOnly(2025, 12, 11), NumAdults = 2, NumChildren = 1, TotalPrice = 40.00m };
                context.Bookings.Add(booking);
                context.SaveChanges();
            }

            var parkRepository = new ParkRepository(new DatabaseContext(options));
            var bookingRepository = new BookingRepository(new DatabaseContext(options));
            var parkService = new ParkService(parkRepository);
            var bookingService = new BookingService(bookingRepository, parkRepository);

            var resultBooking = await bookingService.GetParkBookings(3);

            //Assert
            Assert.NotNull(resultBooking);
            Assert.Equal(10, resultBooking.First().Id);
        }
   
       [Fact]
        public async Task Cannot_Get_A_Booking_With_Invalid_ParkId()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;


            using (var context = new DatabaseContext(options))
            {
                var booking = new Booking { Id = 10, CartId = null, ParkId = 3, Date = new DateOnly(2025, 12, 15), NumAdults = 2, NumChildren = 1, TotalPrice = 40.00m };
                context.Bookings.Add(booking);
                context.SaveChanges();
            }

            var parkRepository = new ParkRepository(new DatabaseContext(options));
            var bookingRepository = new BookingRepository(new DatabaseContext(options));
            var parkService = new ParkService(parkRepository);
            var bookingService = new BookingService(bookingRepository, parkRepository);

            var resultBooking = await bookingService.GetParkBookings(5);

            //Assert
            Assert.Empty(resultBooking);
        }
        [Fact]
        public async Task Can_Get_All_Bookings()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                 .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                 .ToString())
                 .Options;

            var parks = new List<Park>
                {
                    new Park { Id = 1, Name = "Test Park"},
                    new Park { Id = 3, Name = "Test Park 3"}
                };
            var bookings = new List<Booking>
                {
                    new Booking { Id = 10, CartId = null, ParkId = 3, Date = new DateOnly(2025, 12, 25), NumAdults = 2, NumChildren = 1, TotalPrice = 40.00m },
                    new Booking {Id = 11, CartId =null, ParkId = 1, Date = new DateOnly(2025, 12, 25), NumAdults = 3, NumChildren = 0, TotalPrice = 45.00m}
                };

            using (var context = new DatabaseContext(options))
            {
                context.Parks.AddRange(parks);
                context.Bookings.AddRange(bookings);
                context.SaveChanges();
            }

            var parkRepository = new ParkRepository(new DatabaseContext(options));
            var bookingRepository = new BookingRepository(new DatabaseContext(options));
            var parkService = new ParkService(parkRepository);
            var bookingService = new BookingService(bookingRepository, parkRepository);

            var resultBooking = await bookingService.GetBookings();

            //Assert

            foreach (var expectedBooking in bookings)
            {
               var actualBooking = resultBooking.FirstOrDefault(booking => booking.Id == expectedBooking.Id);
               Assert.NotNull(actualBooking);
               Assert.Equal(expectedBooking.NumAdults, actualBooking.NumAdults);
            }
        }


        [Fact]
        public async Task Can_Add_Booking()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid()
               .ToString())
               .Options;

            var park = new Park { Id = 1, Name = "Park One", Description = "There are a lot of trees.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m };
            var booking = new BookingInputDTO { Date = new DateOnly(2025, 11, 11), NumAdults = 3, NumChildren = 0 };

            using (var context = new DatabaseContext(options))
            {
                context.Parks.Add(park);
                context.SaveChanges();
            }

            var parkRepository = new ParkRepository(new DatabaseContext(options));
            var bookingRepository = new BookingRepository(new DatabaseContext(options));
            var parkService = new ParkService(parkRepository);
            var bookingService = new BookingService(bookingRepository, parkRepository);

            bool isAdded = await bookingService.CreateBooking(1, booking);

            //Assert
            Assert.True(isAdded);
        }

        [Fact]
        public async Task Can_Remove_Booking()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid()
               .ToString())
               .Options;

            var park = new Park { Id = 1, Name = "Test Park" };
            var booking = new Booking { Id = 11, CartId = null, ParkId = 1, Date = new DateOnly(2026, 2, 4), NumAdults = 3, NumChildren = 0, TotalPrice = 45.00m };

            using (var context = new DatabaseContext(options))
            {
                context.Parks.Add(park);
                context.Bookings.Add(booking);
               context.SaveChanges();
            }

            var parkRepository = new ParkRepository(new DatabaseContext(options));
            var bookingRepository = new BookingRepository(new DatabaseContext(options));
            var parkService = new ParkService(parkRepository);
            var bookingService = new BookingService(bookingRepository, parkRepository);

            bool isRemoved = await bookingService.RemoveBooking(11);
            var resultBooking = await bookingService.GetParkBookings(1);

            Assert.True(isRemoved);
            Assert.Empty(resultBooking);
        }

    }
}


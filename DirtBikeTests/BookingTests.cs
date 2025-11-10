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
            var booking = new Booking { Id = 10, CartId = null, ParkId = 3, Date = "01-01-2001", NumAdults = 2, NumChildren = 1, TotalPrice = 40.00m };


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
                var booking = new Booking { Id = 10, CartId = null, ParkId = 3, Date = "01-01-2001", NumAdults = 2, NumChildren = 1, TotalPrice = 40.00m };
                context.Bookings.Add(booking);
                context.SaveChanges();
            }

            var bookingService = new BookingService(new DatabaseContext(options));

            List<Booking> resultBooking = await bookingService.GetBooking(3);

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
                var booking = new Booking { Id = 10, CartId = null, ParkId = 3, Date = "01-01-2001", NumAdults = 2, NumChildren = 1, TotalPrice = 40.00m };
                context.Bookings.Add(booking);
                context.SaveChanges();
            }

            var bookingService = new BookingService(new DatabaseContext(options));

            List<Booking> resultBooking = await bookingService.GetBooking(5);

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

            var bookings = new List<Booking>
                {
                    new Booking { Id = 10, CartId = null, ParkId = 3, Date = "01-01-2001", NumAdults = 2, NumChildren = 1, TotalPrice = 40.00m },
                    new Booking {Id = 11, CartId =null, ParkId = 1, Date = "01-01-2001", NumAdults = 3, NumChildren = 0, TotalPrice = 45.00m}

                };

            using (var context = new DatabaseContext(options))
            {

                context.Bookings.AddRange(bookings);
                context.SaveChanges();
            }

            var bookingService = new BookingService(new DatabaseContext(options));

            List<Booking> resultBooking = await bookingService.GetBookings();

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

            var booking = new Booking { Id = 11, CartId = null, ParkId = 1, Date = "01-01-2001", NumAdults = 3, NumChildren = 0, TotalPrice = 45.00m };

            var bookingService = new BookingService(new DatabaseContext(options));

            bool isAdded = await bookingService.CreateBooking(booking);

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

            var booking = new Booking { Id = 11, CartId = null, ParkId = 1, Date = "01-01-2001", NumAdults = 3, NumChildren = 0, TotalPrice = 45.00m };

            using (var context = new DatabaseContext(options))
            {
               context.Bookings.Add(booking);
               context.SaveChanges();
            }

            var bookingService = new BookingService(new DatabaseContext(options));

            bool isAdded = await bookingService.RemoveBooking(11);
            List<Booking> resultBooking = await bookingService.GetBooking(1);

            Assert.True(isAdded);
            Assert.Empty(resultBooking);
        }

    }
}


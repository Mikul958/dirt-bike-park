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
    public class CartTests
    {
        [Fact]
        public void Can_Create_Cart()
        {
            var emptyCart = new Cart();
            var cart = new Cart { Id = Guid.NewGuid() };

            Assert.NotNull(cart);
            Assert.IsType<Cart>(cart);
            Assert.NotEqual(emptyCart, cart);
        }
        
        [Fact]
        public async Task Can_Get_A_Cart_With_Valid_Guid()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;

            var guid = Guid.NewGuid();
            var cart = new Cart { Id = guid };

            using (var context = new DatabaseContext(options))
            {
                context.Carts.Add(cart);
                context.SaveChanges();
            }

            var cartService = new CartService(new DatabaseContext(options));

            Cart resultCart = await cartService.GetCart(guid);

            //Assert
            Assert.NotNull(resultCart);
            Assert.Equal(guid, resultCart.Id);
        }
        
        [Fact]
        public async Task Cannot_Get_A_Cart_With_Invalid_Guid()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                .ToString())
                .Options;


            var guid = Guid.NewGuid();
            var cart = new Cart { Id = guid };

            using (var context = new DatabaseContext(options))
            {
                context.Carts.Add(cart);
                context.SaveChanges();
            }

            var cartService = new CartService(new DatabaseContext(options));

            Cart resultCart = await cartService.GetCart(Guid.Empty);

            //Assert
            Assert.NotEqual(Guid.Empty, resultCart.Id);
        }
        
        [Fact]
        public async Task Can_Get_Add_Booking_To_Cart()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                 .UseInMemoryDatabase(databaseName: Guid.NewGuid()
                 .ToString())
                 .Options;

            var park = new Park { Id = 3, Name = "Park Three", Description = "It's pretty green.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m };
            var guid = Guid.NewGuid();
            var cart = new Cart { Id = guid };
            var booking = new Booking { Id = 10, CartId = Guid.Empty, ParkId = 3, Date = "01-01-2001", NumAdults = 2, NumChildren = 1, TotalPrice = 40.00m };


            using (var context = new DatabaseContext(options))
            {

                context.Bookings.Add(booking);
                context.Carts.Add(cart);
                context.Parks.Add(park);
                context.SaveChanges();
            }

            var cartService = new CartService(new DatabaseContext(options));

            bool isAdded = await cartService.AddBookingToCart(guid, 3, booking);

            //Assert

            //foreach (var expectedBooking in booking)
            //{
            //    var actualBooking = resultBooking.FirstOrDefault(booking => booking.Id == expectedBooking.Id);
            //    Assert.NotNull(actualBooking);
            //    Assert.Equal(expectedBooking.NumAdults, actualBooking.NumAdults);
            //}
        }

        /*
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
        */
    }
}


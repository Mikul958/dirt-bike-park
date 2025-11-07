using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.EntityFrameworkCore;

namespace DirtBikePark.Services
{
    public class CartService : ICartService
    {
        private readonly DatabaseContext _context;
        public CartService(DatabaseContext context)
        {
            _context = context;
        }
        public Task<Cart> GetCart(Guid? cartId)
        {
            // TODO call DatabaseContext and implement logic there.

            var cart = _context.Carts
                .Include(c => c.Bookings)
                .FirstOrDefault(c => c.Id == cartId);

            //Cart mockCart = new Cart();
            //mockCart.Id = Guid.NewGuid();

            //Booking mockBooking = new Booking();
            //mockBooking.Id = 10;
            //mockBooking.CartId = mockCart.Id;
            //mockBooking.ParkId = 100;
            //mockBooking.Date = "01-01-2001";
            //mockBooking.NumAdults = 2;
            //mockBooking.NumChildren = 0;
            //mockBooking.TotalPrice = 10.98m;

            //mockCart.Bookings.Add(mockBooking);

            return Task.FromResult(cart);
        }

        public Task<bool> AddBookingToCart(Guid cartId, int parkId, Booking bookingInfo)
        {
            if (_context.Carts.Find(cartId) == null)
                return Task.FromResult(false);

            if (_context.Parks.Find(parkId) == null)
                return Task.FromResult(false);

            if (bookingInfo == null)
                return Task.FromResult(false);

            bookingInfo.CartId = cartId;
            bookingInfo.ParkId = parkId;

            var cart = _context.Carts.Find(cartId);
            cart.Bookings.Add(bookingInfo);
            _context.SaveChanges();

            // Maybe add more sanity checks to ensure that the same booking hasn't been made in the same cart

            return Task.FromResult(true);
        }
        public Task<bool> RemoveBookingFromCart(Guid cartId, int bookingId)
        {
            var cart = _context.Carts
                .Include(c => c.Bookings)
                .FirstOrDefault(c => c.Id == cartId);
            if (cart == null)
                return Task.FromResult(false);

            var booking = cart.Bookings.Find(b => b.Id == bookingId);
            if (booking == null)
                return Task.FromResult(false);

            cart.Bookings.Remove(booking);
            booking.CartId = Guid.Empty; // Maybe make this id nullable?
            _context.SaveChanges();

            return Task.FromResult(true);
        }
    }
}

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
            // If a cartId was not provided, generate a new Guid and assign it
            if (cartId == null)
                cartId = Guid.NewGuid();
            
            // Retrieve the cart with the given ID from the database if it exists
            var cart = _context.Carts
                .Include(c => c.Bookings)
                .FirstOrDefault(c => c.Id == cartId);

            // If not, create a cart with the new Guid and save it to the database
            if (cart == null)
            {
                cart = new Cart();
                cart.Id = (Guid)cartId;

                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            return Task.FromResult(cart);
        }

        public Task<bool> AddBookingToCart(Guid cartId, int parkId, Booking bookingInfo)
        {
            var cart = _context.Carts
                .Include(c => c.Bookings)
                .FirstOrDefault(c => c.Id == cartId);

            if (cart == null)
                return Task.FromResult(false);

            if (_context.Parks.Find(parkId) == null)
                return Task.FromResult(false);

            if (bookingInfo == null)
                return Task.FromResult(false);

            bookingInfo.CartId = cartId;
            bookingInfo.ParkId = parkId;

           
            cart.Bookings.Add(bookingInfo);
            _context.Bookings.Add(bookingInfo);  // Might delete later after merge with BookingService Class implemented
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

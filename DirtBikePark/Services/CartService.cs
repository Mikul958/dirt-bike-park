using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;

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

            Cart mockCart = new Cart();
            mockCart.Id = Guid.NewGuid();

            Booking mockBooking = new Booking();
            mockBooking.Id = 10;
            mockBooking.CartId = mockCart.Id;
            mockBooking.ParkId = 100;
            mockBooking.Date = "01-01-2001";
            mockBooking.NumAdults = 2;
            mockBooking.NumChildren = 0;
            mockBooking.TotalPrice = 10.98m;

            mockCart.Bookings.Add(mockBooking);

            return Task.FromResult(mockCart);
        }

        public Task<bool> AddBookingToCart(Guid cartId, int parkId, Booking bookingInfo)
        {
            return Task.FromResult(true);
        }
        public Task<bool> RemoveBookingFromCart(Guid cartId, int bookingId)
        {
            return Task.FromResult(false);
        }
    }
}

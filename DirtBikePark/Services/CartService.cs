using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.EntityFrameworkCore;

namespace DirtBikePark.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkRepository _parkRepository;

        public CartService(ICartRepository cartRepository, IBookingRepository bookingRepository, IParkRepository parkRepository)
        {
            _cartRepository = cartRepository;
            _bookingRepository = bookingRepository;
            _parkRepository = parkRepository;
        }

        public Task<Cart> GetCart(Guid? cartId)
        {
            // If a cartId was not provided, generate a new Guid and assign it
            if (cartId == null)
                cartId = Guid.NewGuid();

            // Retrieve the cart with the given ID from the database if it exists
            Cart? cart = _cartRepository.GetCart(cartId);

            // If not, create a cart with the new Guid and save it to the database
            if (cart == null)
            {
                cart = new Cart();
                cart.Id = (Guid)cartId;

                _cartRepository.AddCart(cart);
                _cartRepository.Save();
            }

            return Task.FromResult(cart);
        }

        public Task<bool> AddBookingToCart(Guid cartId, int parkId, int bookingId)
        {
            // Check that the provided park exists
            if (_parkRepository.GetPark(parkId) == null)
                throw new InvalidOperationException($"Park with ID {parkId} not found.");

            // Check that the provided booking exists and is not already in a cart
            Booking? retrievedBooking = _bookingRepository.GetBooking(bookingId);
            if (retrievedBooking == null || retrievedBooking.CartId != null)
                throw new InvalidOperationException($"Booking with ID {bookingId} not found.");

            // Check that the provided cart exists
            Cart? retrievedCart = _cartRepository.GetCart(cartId);
            if (retrievedCart == null)
                throw new InvalidOperationException($"Cart with ID {cartId} not found.");

            // Update parkId and cartId with provided values
            retrievedBooking.CartId = cartId;
            retrievedBooking.ParkId = parkId;

            // Adding Bookings price to cart total price
            retrievedCart.TotalPrice += retrievedBooking.TotalPrice;
            _bookingRepository.Save();

            return Task.FromResult(true);
        }
        public Task<bool> RemoveBookingFromCart(Guid cartId, int bookingId)
        {
            // Check that the provided booking exists and is already in the provided cart
            Booking? retrievedBooking = _bookingRepository.GetBooking(bookingId);
            if (retrievedBooking == null || retrievedBooking.CartId != cartId)
                throw new InvalidOperationException($"Booking with ID {bookingId} not found.");

            // Wipe the cartId to break the link between booking and cart
            retrievedBooking.CartId = null;

            // Check that the provided cart exists
            Cart? retrievedCart = _cartRepository.GetCart(cartId);
            if (retrievedCart == null)
                throw new InvalidOperationException($"Cart with ID {cartId} not found.");

            // Remove the bookings price from the carts total price
            retrievedCart.TotalPrice -= retrievedBooking.TotalPrice;
            _bookingRepository.Save();

            return Task.FromResult(true);
        }
    }
}

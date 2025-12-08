using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Numerics;
using System.Text.RegularExpressions;

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

        public Task<CartResponseDTO> GetCart(Guid? cartId)
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

            CartResponseDTO cartResponse = new CartResponseDTO(cart);
            return Task.FromResult(cartResponse);
        }

        public Task<bool> AddBookingToCart(Guid cartId, int parkId, int bookingId)
        {
            // Check that the provided park exists
            if (_parkRepository.GetPark(parkId) == null)
                throw new InvalidOperationException($"Park with ID {parkId} not found.");

            // Check that the provided booking exists and is not already in a cart
            Booking? retrievedBooking = _bookingRepository.GetBooking(bookingId);
            if (retrievedBooking == null)
                throw new InvalidOperationException($"Booking with ID {bookingId} not found.");
            if (retrievedBooking.CartId != null)
                throw new InvalidOperationException($"Booking with ID {bookingId} is already in a cart.");

            // Check that the provided cart exists
            Cart? retrievedCart = _cartRepository.GetCart(cartId);
            if (retrievedCart == null)
                throw new InvalidOperationException($"Cart with ID {cartId} not found.");

            // Update parkId and cartId with provided values
            retrievedBooking.CartId = cartId;
            retrievedBooking.ParkId = parkId;
            _bookingRepository.Save();

            return Task.FromResult(true);
        }
        public Task<bool> RemoveBookingFromCart(Guid cartId, int bookingId)
        {
            // Check that the provided booking exists and is already in the provided cart
            Booking? retrievedBooking = _bookingRepository.GetBooking(bookingId);
            if (retrievedBooking == null)
                throw new InvalidOperationException($"Booking with ID {bookingId} not found.");
            if (retrievedBooking.CartId != cartId)
                throw new InvalidOperationException($"Booking with ID {bookingId} is not in the specified cart.");

            // Check that the provided cart exists
            Cart? retrievedCart = _cartRepository.GetCart(cartId);
            if (retrievedCart == null)
                throw new InvalidOperationException($"Cart with ID {cartId} not found.");

            // Wipe the cartId to break the link between booking and cart
            retrievedBooking.CartId = null;
            _bookingRepository.Save();

            return Task.FromResult(true);
        }

        public Task<bool> ProcessPayment(Guid cartId, PaymentInfo paymentInfo)
        {
            // Check that the provided cart exists and contains at least one booking
            Cart? retrievedCart = _cartRepository.GetCart(cartId);
            if (retrievedCart == null || retrievedCart.Bookings.Count == 0)
                throw new InvalidOperationException("Cart with ID {cartId} does not exist or is empty.");

            // Validate credit card number
            if (paymentInfo.CardNumber.IsNullOrEmpty())
                throw new InvalidOperationException("No card number provided.");
            paymentInfo.CardNumber = Regex.Replace(paymentInfo.CardNumber, "[^0-9]", "");
            if (!LuhnCheck(paymentInfo.CardNumber))
                throw new InvalidOperationException("Invalid card number.");

            // Validate CCV
            int parsedCCV = 0;
            if (paymentInfo.Ccv.Length < 3 || paymentInfo.Ccv.Length > 4 || !int.TryParse(paymentInfo.Ccv, out parsedCCV))
                throw new InvalidOperationException("Invalid CCV.");
            if (parsedCCV < 0 || parsedCCV > 9999)
                throw new InvalidOperationException("Invalid CCV.");

            // Validate name
            // Nothing to do here I guess

            // Ensure expiration date has not passed
            if (paymentInfo.ExpirationDate < DateOnly.FromDateTime(DateTime.Now))
                throw new InvalidOperationException("Invalid expiration date.");

            // Mark all cart bookings as true in database
            _cartRepository.FinalizePayment(retrievedCart);
            _cartRepository.Save();

            return Task.FromResult(true);
        }

        private bool LuhnCheck(string cardNumber)
        {
            int luhnSum = 0;
            bool doubleDigit = false;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int current = int.Parse(cardNumber[i].ToString());
                if (doubleDigit)
                    current *= 2;                        // Multiply every second digit by 2 (right to left, starting with no double)
                luhnSum += current / 10 + current % 10;  // Add digits of current element to total
                doubleDigit = !doubleDigit;
            }
            
            return (luhnSum % 10) == 0;
        }
    }
}

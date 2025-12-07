using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface ICartService
    {
        public Task<Cart> GetCart(Guid? cartId);
        public Task<bool> AddBookingToCart(Guid cartId, int parkId, int bookingId);
        public Task<bool> RemoveBookingFromCart(Guid cartId, int bookingId);
        public Task<bool> ProcessPayment(Guid cartId, PaymentInfo paymentInfo);
    }
}

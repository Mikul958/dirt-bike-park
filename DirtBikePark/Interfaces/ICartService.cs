using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface ICartService
    {
        public Task<CartResponseDTO> GetCart(Guid? cartId);
        public Task<bool> AddBookingToCart(Guid cartId, int parkId, int bookingId);
        public Task<bool> RemoveBookingFromCart(Guid cartId, int bookingId);
    }
}

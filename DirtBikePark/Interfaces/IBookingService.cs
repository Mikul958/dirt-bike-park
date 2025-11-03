using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IBookingService
    {
        Task<Booking?> GetBookingAsync(Guid cartId, int bookingId);
        Task<IReadOnlyList<Booking>> GetBookingsAsync(Guid cartId);
        Task<Booking> CreateBookingAsync(Guid cartId, Booking booking);
        Task<bool> RemoveBookingAsync(Guid cartId, int bookingId);
    }
}

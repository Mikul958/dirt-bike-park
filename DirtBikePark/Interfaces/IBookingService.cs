using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IBookingService
    {
        Task<IReadOnlyList<Booking>> GetBookingsAsync();                 // GetBookings()
        Task<IReadOnlyList<Booking>> GetBookingsByParkAsync(int parkId); // GetBooking(parkId) -> list
        Task<Booking> CreateBookingAsync(int parkId, Booking booking);   // CreateBooking(parkId)
        Task<bool> RemoveBookingAsync(int bookingId);                    // RemoveBooking(bookingId)
    }
}


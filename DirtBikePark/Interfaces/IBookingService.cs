using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetBookings();
        Task<IEnumerable<Booking>> GetBooking(int parkId);
        Task<bool> CreateBooking(Booking booking);
        Task<bool> RemoveBooking(int bookingId);
    }
}


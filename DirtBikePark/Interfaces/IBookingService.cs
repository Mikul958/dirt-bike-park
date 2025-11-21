using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetBookings();
        Task<IEnumerable<Booking>> GetParkBookings(int parkId);
        Task<Booking?> GetBooking(int bookingId);
        Task<bool> CreateBooking(Booking booking);
        Task<bool> RemoveBooking(int bookingId);
    }
}


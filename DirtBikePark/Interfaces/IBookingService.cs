using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IBookingService
    {
        Task<List<Booking>> GetBookings();
        Task<List<Booking>> GetBooking(int parkId);
        Task<bool> CreateBooking(Booking booking);
        Task<bool> RemoveBooking(int bookingId);
    }
}


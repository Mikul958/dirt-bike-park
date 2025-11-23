using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IBookingRepository
    {
        Booking? GetBooking(int bookingId);
        IEnumerable<Booking> GetBookingsByPark(int parkId);
        IEnumerable<Booking> GetBookings();
        void AddBooking(Booking booking);
        void RemoveBooking(Booking booking);
        void Save();
    }
}

using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IBookingRepository
    {
        Booking? GetBooking(int bookingId);
        IEnumerable<Booking> GetBookingsByPark(int parkId);
        IEnumerable<Booking> GetBookings();
        int CountGuestsForPark(int parkId, DateOnly date);
        void AddBooking(Booking booking);
        void RemoveBooking(Booking booking);
        void Save();
    }
}

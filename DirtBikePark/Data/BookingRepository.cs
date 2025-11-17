using DirtBikePark.Interfaces;
using DirtBikePark.Models;

namespace DirtBikePark.Data
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DatabaseContext _context;
        public BookingRepository(DatabaseContext context)
        {
            _context = context;
        }
        public void AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
        }

        public Booking? GetBooking(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(booking => booking.Id == bookingId);
            return booking;
        }

        public IEnumerable<Booking> GetBookings()
        {
            return _context.Bookings;
        }

        public IEnumerable<Booking> GetBookingsByPark(int parkId)
        {
            var bookings = _context.Bookings.Where(booking => booking.ParkId == parkId);
            return bookings;
        }

        public void RemoveBooking(Booking booking)
        {
            _context.Bookings.Remove(booking);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

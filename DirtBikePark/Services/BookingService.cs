using System.Collections.Concurrent;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;

namespace DirtBikePark.Services
{
    public class BookingService : IBookingService
    {
        // All bookings keyed by booking Id
        private static readonly ConcurrentDictionary<int, Booking> _bookings = new();
        private static int _nextId = 1;
        private static readonly object _idLock = new();

        public Task<IReadOnlyList<Booking>> GetBookingsAsync()
        {
            var list = _bookings.Values.OrderBy(b => b.Id).ToList().AsReadOnly();
            return Task.FromResult<IReadOnlyList<Booking>>(list);
        }

        public Task<IReadOnlyList<Booking>> GetBookingsByParkAsync(int parkId)
        {
            var list = _bookings.Values
                .Where(b => b.ParkId == parkId)
                .OrderBy(b => b.Id)
                .ToList()
                .AsReadOnly();

            return Task.FromResult<IReadOnlyList<Booking>>(list);
        }

        public Task<Booking> CreateBookingAsync(int parkId, Booking booking)
        {
            // Server-assign identity Id
            int id;
            lock (_idLock) { id = _nextId++; }

            booking.Id = id;
            booking.ParkId = parkId;

            // Ensure CartId exists since model requires it; generate if missing/default.
            if (booking.CartId == Guid.Empty)
            {
                booking.CartId = Guid.NewGuid();
            }

            _bookings[booking.Id] = booking;
            return Task.FromResult(booking);
        }

        public Task<bool> RemoveBookingAsync(int bookingId)
        {
            return Task.FromResult(_bookings.TryRemove(bookingId, out _));
        }
    }
}

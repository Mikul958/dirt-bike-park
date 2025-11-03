using System.Collections.Concurrent;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;


namespace DirtBikePark.Services
{
    /// <summary>
    /// Simple in-memory implementation keyed by CartId.
    /// Replace with EF Core later without changing controller signatures.
    /// </summary>
    public class BookingService : IBookingService
    {
        // cartId -> bookings
        private static readonly ConcurrentDictionary<Guid, ConcurrentDictionary<int, Booking>> _store
            = new();

        private static int _nextId = 1;
        private static readonly object _idLock = new();

        public Task<Booking?> GetBookingAsync(Guid cartId, int bookingId)
        {
            if (_store.TryGetValue(cartId, out var bookings) &&
                bookings.TryGetValue(bookingId, out var booking))
            {
                return Task.FromResult<Booking?>(booking);
            }
            return Task.FromResult<Booking?>(null);
        }

        public Task<IReadOnlyList<Booking>> GetBookingsAsync(Guid cartId)
        {
            if (_store.TryGetValue(cartId, out var bookings))
            {
                var list = bookings.Values.OrderBy(b => b.Id).ToList().AsReadOnly();
                return Task.FromResult<IReadOnlyList<Booking>>(list);
            }
            return Task.FromResult<IReadOnlyList<Booking>>(Array.Empty<Booking>());
        }

        public Task<Booking> CreateBookingAsync(Guid cartId, Booking booking)
        {
            // Ensure store for this cart
            var bucket = _store.GetOrAdd(cartId, _ => new ConcurrentDictionary<int, Booking>());

            // Server-assign identity Id (matches [DatabaseGenerated] on model).
            int id;
            lock (_idLock) { id = _nextId++; }
            booking.Id = id;
            booking.CartId = cartId;

            bucket[booking.Id] = booking;
            return Task.FromResult(booking);
        }

        public Task<bool> RemoveBookingAsync(Guid cartId, int bookingId)
        {
            if (_store.TryGetValue(cartId, out var bookings))
            {
                return Task.FromResult(bookings.TryRemove(bookingId, out _));
            }
            return Task.FromResult(false);
        }
    }
}

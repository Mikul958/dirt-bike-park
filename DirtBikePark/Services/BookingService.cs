using System.Collections.Concurrent;
using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DirtBikePark.Services
{
    public class BookingService : IBookingService
    {
        private readonly DatabaseContext _context;
        public BookingService(DatabaseContext context)
        {
            _context = context;
        }

        public Task<List<Booking>> GetBookings()
        {
            // Retrieve all finalized bookings from the database with no respect to cart or park
            List<Booking> bookings = _context.Bookings
                .ToList();
            return Task.FromResult(bookings);
        }

        public Task<List<Booking>> GetBooking(int parkId)
        {
            // Retrieve all finalized bookings with the given park ID
            List<Booking> bookingsWithPark = _context.Bookings
                .Where(booking => booking.ParkId == parkId)
                .ToList();
            return Task.FromResult(bookingsWithPark);
        }
        public Task<Booking?> GetBookingFromId(int bookingId)
        {
            // Retrieve finalized booking from its ID
            Booking? bookingWithId = _context.Bookings
                .FirstOrDefault(booking => booking.Id == bookingId);
            return Task.FromResult(bookingWithId);
        }

        public Task<bool> CreateBooking(Booking booking)
        {
            // Check if a park with the given parkID exists in the database
            if (!_context.Parks.Where(park => park.Id == booking.ParkId).Any())
                return Task.FromResult(false);
            
            // Wipe ID field so that the database can generate an ID automatically and cartID field
            booking.Id = 0;
            booking.CartId = null;

            // Add the booking to the database
            _context.Bookings.Add(booking);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> RemoveBooking(int bookingId)
        {
            // Reject if ID is invalid
            if (bookingId < 0)
                return Task.FromResult(false);

            // Check if there is a park in the database with the given ID and return failure if not
            Booking? booking = _context.Bookings.FirstOrDefault(booking => booking.Id == bookingId);
            if (booking == null)
                return Task.FromResult(false);

            // Remove the booking from the database
            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
    }
}

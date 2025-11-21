using System.Collections.Concurrent;
using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DirtBikePark.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkRepository _parkRepository;
        public BookingService(IBookingRepository bookingRepository, IParkRepository parkRepository)
        {
            _bookingRepository = bookingRepository;
            _parkRepository = parkRepository;
        }

        public Task<IEnumerable<Booking>> GetBookings()
        {
            // Retrieve all finalized bookings from the database with no respect to cart or park
            var bookings = _bookingRepository.GetBookings();
            return Task.FromResult(bookings);
        }

        public Task<IEnumerable<Booking>> GetParkBookings(int parkId)
        {
            // Retrieve all finalized bookings with the given park ID
            var bookingsWithPark = _bookingRepository.GetBookingsByPark(parkId);
            return Task.FromResult(bookingsWithPark);
        }

        public Task<Booking?> GetBooking(int bookingId)
        {
            // Retrieve the booking with the given park ID (or null)
            var booking = _bookingRepository.GetBooking(bookingId);
            return Task.FromResult(booking);
        }

        public Task<bool> CreateBooking(Booking booking)
        {

            // Check if a park with the given parkID exists in the database
            if(_parkRepository.GetPark(booking.ParkId) == null)
                return Task.FromResult(false);
            
            // Wipe ID field so that the database can generate an ID automatically and cartID field
            booking.Id = 0;
            booking.CartId = null;

            // Add the booking to the database
            _bookingRepository.AddBooking(booking);
            _bookingRepository.Save();
            return Task.FromResult(true);
        }

        public Task<bool> RemoveBooking(int bookingId)
        {
            // Reject if ID is invalid
            if (bookingId < 0)
                return Task.FromResult(false);

            // Check if there is a booking in the database with the given ID and return failure if not
            Booking? booking = _bookingRepository.GetBooking(bookingId);
            if (booking == null)
                return Task.FromResult(false);

            // Remove the booking from the database
            _bookingRepository.RemoveBooking(booking);
            _bookingRepository.Save();
            return Task.FromResult(true);
        }
    }
}

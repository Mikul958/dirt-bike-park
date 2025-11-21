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

        public Task<IEnumerable<BookingResponseDTO>> GetBookings()
        {
            // Retrieve all finalized bookings from the database with no respect to cart or park
            IEnumerable<BookingResponseDTO> bookings = _bookingRepository
                .GetBookings()
                .Select(booking => new BookingResponseDTO(booking));
            return Task.FromResult(bookings);
        }

        public Task<IEnumerable<BookingResponseDTO>> GetParkBookings(int parkId)
        {
            // Retrieve all finalized bookings with the given park ID
            IEnumerable<BookingResponseDTO> bookingsWithPark = _bookingRepository
                .GetBookingsByPark(parkId)
                .Select(booking => new BookingResponseDTO(booking));
            return Task.FromResult(bookingsWithPark);
        }

        public Task<BookingResponseDTO?> GetBooking(int bookingId)
        {
            // Retrieve the booking with the given park ID (or null)
            Booking? booking = _bookingRepository.GetBooking(bookingId);
            BookingResponseDTO? bookingResponse = booking != null ? new BookingResponseDTO(booking) : null;
            return Task.FromResult(bookingResponse);
        }

        public Task<bool> CreateBooking(int parkId, BookingInputDTO bookingInfo)
        {
            // Check if a park with the given parkID exists in the database
            if(_parkRepository.GetPark(parkId) == null)
                return Task.FromResult(false);

            // Create a new booking with the given parkId and bookingInfo (no cart info allowed at this stage)
            Booking booking = bookingInfo.FromInputDTO();
            booking.ParkId = parkId;

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

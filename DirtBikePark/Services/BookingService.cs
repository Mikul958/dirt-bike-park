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

        public Task<BookingResponseDTO> CreateBooking(int parkId, BookingInputDTO bookingInfo)
        {
            // Check if a park with the given parkID exists in the database
            Park? park = _parkRepository.GetPark(parkId);
            if (park == null)
                throw new InvalidOperationException($"Park with ID {parkId} not found.");

            // Validating BookingDTO input matches requested Park info  --might remove if guestLimit is related to number of bookings versus actual guest in a booking
            int totalGuest = bookingInfo.NumAdults + bookingInfo.NumChildren;
            if (totalGuest > park.GuestLimit)
                throw new InvalidOperationException($"Cannot add this booking to this park as it has cannot fullfill your capacity: {park.GuestLimit} remaining spots. Your guest count: {totalGuest}");
            if (totalGuest == 0)
                throw new InvalidOperationException($"Cannot create a Booking with no guests: {bookingInfo.NumChildren} children, {bookingInfo.NumAdults} adults");

            // Reduce guestLimit
            park.GuestLimit -= totalGuest;

            // Calculate price
            decimal adultCost = bookingInfo.NumAdults * park.PricePerAdult;
            decimal childrenCost = bookingInfo.NumChildren * park.PricePerChild;

            // Create a new booking with the given parkId and bookingInfo (no cart info allowed at this stage)
            Booking booking = bookingInfo.FromInputDTO();
            booking.ParkId = parkId;
            booking.TotalPrice = adultCost + childrenCost;

            // Add the booking to the database
            _bookingRepository.AddBooking(booking);
            _bookingRepository.Save();
            return Task.FromResult(new BookingResponseDTO(booking));
        }

        public Task<bool> RemoveBooking(int bookingId)
        {
            // Check if there is a booking in the database with the given ID and return failure if not
            Booking? booking = _bookingRepository.GetBooking(bookingId);
            if (booking == null)
                throw new InvalidOperationException($"Booking with ID {bookingId} not found.");

            // Remove the booking from the database
            _bookingRepository.RemoveBooking(booking);
            _bookingRepository.Save();
            return Task.FromResult(true);
        }
    }
}

using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DirtBikePark.Services
{
	public class ParkService : IParkService
    {
		private readonly IParkRepository _parkRepository;
        private readonly IBookingRepository _bookingRepository;

        public ParkService(IParkRepository parkRepository, IBookingRepository bookingRepository)
        {
			_parkRepository = parkRepository;
            _bookingRepository = bookingRepository;
        }

        public Task<ParkResponseDTO> GetPark(int parkId)
		{
            Park? park = _parkRepository.GetPark(parkId);
            ParkResponseDTO? parkResponse = (park != null ? new ParkResponseDTO(park) : null);
            if (parkResponse == null)
                throw new InvalidOperationException($"Park with ID {parkId} not found.");
            return Task.FromResult(parkResponse);
		}
		
		public Task<IEnumerable<ParkResponseDTO>> GetParks()
		{
            IEnumerable<ParkResponseDTO> parks = _parkRepository
                .GetParks()
                .Select(park => new ParkResponseDTO(park));

            if (parks.IsNullOrEmpty())
                throw new InvalidOperationException("There are no parks in the database currently");

            return Task.FromResult(parks);
        }

        public Task<ParkResponseDTO> AddPark(ParkInputDTO parkInfo)
        {
            // Validate that the park exists and it has been created with a name
            if (parkInfo == null || string.IsNullOrWhiteSpace(parkInfo.Name))
                throw new ArgumentException("Park itself nor its Park name can be null or empty.");

            // Validate that prices are reasonable, an adult's entry is greater than or equals to a child's entry
            if (parkInfo.PricePerAdult < parkInfo.PricePerChild)
                throw new ArgumentException("A child's entry price shouldn't be greater than an adults");

            // Create a new Park with the given parkInfo
            Park park = parkInfo.FromInputDTO();
			
            // Add the new park to the database
			_parkRepository.AddPark(park);
            _parkRepository.Save();
            return Task.FromResult(new ParkResponseDTO(park));
		}

		public Task<bool> RemovePark(int parkId)
		{
            // Check if there is a park in the database with the given ID and return failure if not
            Park? park = _parkRepository.GetPark(parkId);
			if (park == null)
                throw new InvalidOperationException($"Park with ID {parkId} not found.");

            // Remove the park and associated bookings from the database
            _parkRepository.RemovePark(park);
            _parkRepository.Save();
			return Task.FromResult(true);
		}

        public Task<bool> EditPark(int parkId, ParkInputDTO newPark)
        {
            // Validate prices and guestLimit
            if (newPark.PricePerAdult < 0)
                throw new ArgumentException("The price per adult must be a positive value.");
            if (newPark.PricePerChild < 0)
                throw new ArgumentException("The price per adult must be a positive value.");
            if (newPark.GuestLimit <= 0)
                throw new ArgumentException("The guest limit must be at least 1.");

            // Check if there is a park in the database with the given ID and return failure if not
            Park? park = _parkRepository.GetPark(parkId);
            if (park == null)
                throw new InvalidOperationException($"Park with ID {parkId} not found.");

            // Assign values then update and save
            park.PricePerAdult = newPark.PricePerAdult;
            park.PricePerChild = newPark.PricePerChild;
            park.GuestLimit = newPark.GuestLimit;
            _parkRepository.UpdatePark(park);
            _parkRepository.Save();

            return Task.FromResult(true);
        }

        public Task<bool> AddGuestLimitToPark(int parkId, int numberOfGuests)
        {
            // Validate number of guests
            if (numberOfGuests < 0)
                throw new InvalidOperationException("Number of guests must be non-negative.");

            // Retrieve the park from the database, verify it exists
            Park? park = _parkRepository.GetPark(parkId);
            if (park == null)
                throw new InvalidOperationException($"Park with ID {parkId} not found.");

            // Update guest limit and update park in the database
            park.GuestLimit = numberOfGuests;
            _parkRepository.UpdatePark(park);
            _parkRepository.Save();
            return Task.FromResult(true);
        }

        public Task<bool> RemoveGuestsFromPark(int parkId, DateOnly date, int numberOfGuests)
        {
            // Validate parkId
            Park? park = _parkRepository.GetPark(parkId);
            if (park == null)
                throw new InvalidOperationException($"Park with ID {parkId} not found.");

            // Validate/sanitize numberOfGuests
            int guestsInPark = _bookingRepository.CountGuestsForPark(parkId, date);
            if (guestsInPark == 0)
                throw new InvalidOperationException($"Park with currently has no guests on date {date}");
            if (guestsInPark < numberOfGuests)
                numberOfGuests = guestsInPark;

            // Get all bookings that match the given park and date ordered descending by ID and remove until numberOfGuests have been removed
            IEnumerable<Booking> bookings = _bookingRepository.GetBookingsForParkWithDate(parkId, date);
            int guestsRemoved = 0;
            foreach (Booking booking in bookings)
            {
                if (guestsRemoved >= numberOfGuests)
                    break;

                guestsRemoved += booking.NumAdults + booking.NumChildren;
                _bookingRepository.RemoveBooking(booking);
            }
            _bookingRepository.Save();
            return Task.FromResult(true);
        }
	}
}
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
	// Mutable version of the Tuple type
    internal class MutableTuple<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;

        public MutableTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
    
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
            if (newPark.GuestLimit < 0)
                throw new ArgumentException("The guest limit must be at least 0.");

            // Check if there is a park in the database with the given ID and return failure if not
            Park? park = _parkRepository.GetPark(parkId);
            if (park == null)
                throw new InvalidOperationException($"Park with ID {parkId} not found.");

            // Assign values then update and save
            int oldGuestLimit = park.GuestLimit;
            park.PricePerAdult = newPark.PricePerAdult;
            park.PricePerChild = newPark.PricePerChild;
            park.GuestLimit = newPark.GuestLimit;
            _parkRepository.UpdatePark(park);
            _parkRepository.Save();

            // Stretch goal -- remove existing guests over limit on every date
            if (park.GuestLimit < oldGuestLimit)
                RemoveGuestsOverNewLimit(parkId, park.GuestLimit);
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
            int oldGuestLimit = park.GuestLimit;
            park.GuestLimit = numberOfGuests;
            _parkRepository.UpdatePark(park);
            _parkRepository.Save();

            // Stretch goal -- remove existing guests over limit on every date
            if (park.GuestLimit < oldGuestLimit)
                RemoveGuestsOverNewLimit(parkId, park.GuestLimit);
            return Task.FromResult(true);
        }

        private void RemoveGuestsOverNewLimit(int parkId, int newGuestLimit)
        {
            // Maps a date in the database to a list of Bookings on that date as well as an accumulated guest total
            Dictionary<DateOnly, MutableTuple<List<Booking>, int>> bookingMap = new Dictionary<DateOnly, MutableTuple<List<Booking>, int>>();

            // Get all bookings for a park and place them into map based on their date (ordered backwards because higher ID = more recent)
            List<Booking> bookings = _bookingRepository.GetBookingsByPark(parkId).OrderByDescending(booking => booking.Id).ToList();
            foreach (Booking booking in bookings)
            {
                // Initialize a new map entry if the current booking date has not already been encountered
                if (!bookingMap.ContainsKey(booking.Date))
                {
                    MutableTuple<List<Booking>, int> newMapElement = new MutableTuple<List<Booking>, int>(new List<Booking>(), 0);
                    bookingMap.Add(booking.Date, newMapElement);
                }

                // Push the new booking to the entry list and add the guest count to its total
                bookingMap[booking.Date].Item1.Add(booking);
                bookingMap[booking.Date].Item2 += booking.NumAdults + booking.NumChildren;
            }

            // Iterate through each date in map and remove bookings over the park's new guest limit
            List<Booking> bookingsToRemove = new List<Booking>();
            foreach (MutableTuple<List<Booking>, int> bookingMapElement in bookingMap.Values)
            {
                // Exit if count is already below limit
                int currentGuestCount = bookingMapElement.Item2;
                if (currentGuestCount <= newGuestLimit)
                    continue;

                // Iterate through bookings on this date and add to removal list until count falls below limit
                foreach (Booking booking in bookingMapElement.Item1)
                {
                    bookingsToRemove.Add(booking);
                    currentGuestCount -= (booking.NumAdults + booking.NumChildren);

                    if (currentGuestCount <= newGuestLimit)
                        break;
                }
            }

            // Remove all bookings that have been added to remove list
            _bookingRepository.RemoveBookingsInList(bookingsToRemove);
            _bookingRepository.Save();
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
using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
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
        public ParkService(IParkRepository parkRepository)
        {
			_parkRepository = parkRepository;
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

        public Task<bool> AddPark(ParkInputDTO parkInfo)
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
            return Task.FromResult(true);
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
	}
}
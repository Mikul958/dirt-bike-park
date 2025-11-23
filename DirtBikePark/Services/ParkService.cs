using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.EntityFrameworkCore;
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

        public Task<ParkResponseDTO?> GetPark(int parkId)
		{
            Park? park = _parkRepository.GetPark(parkId);
            ParkResponseDTO? parkResponse = park != null ? new ParkResponseDTO(park) : null;
            return Task.FromResult(parkResponse);
		}
		
		public Task<IEnumerable<ParkResponseDTO>> GetParks()
		{
            IEnumerable<ParkResponseDTO> parks = _parkRepository
                .GetParks()
                .Select(park => new ParkResponseDTO(park));
            return Task.FromResult(parks);
        }

        public Task<bool> AddPark(ParkInputDTO parkInfo)
        {
            // Validate that the park exists and it has been created with a name
            if (parkInfo == null || string.IsNullOrWhiteSpace(parkInfo.Name))
                return Task.FromResult(false);

            // Create a new Park with the given parkInfo
            Park park = parkInfo.FromInputDTO();
			
            // Add the new park to the database
			_parkRepository.AddPark(park);
            _parkRepository.Save();
            return Task.FromResult(true);
		}
		
		public Task<bool> RemovePark(int parkId)
		{
            // Reject if parkId is invalid
            if (parkId < 1)
                return Task.FromResult(false);

            // Check if there is a park in the database with the given ID and return failure if not
            Park? park = _parkRepository.GetPark(parkId);
			if (park == null)
				return Task.FromResult(false);

            // Remove the park and associated bookings from the database
            _parkRepository.RemovePark(park);
            _parkRepository.Save();
			return Task.FromResult(true);
		}
	}
}
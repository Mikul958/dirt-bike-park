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

        public Task<Park?> GetPark(int parkId)
		{
            var park = _parkRepository.GetPark(parkId);
            return Task.FromResult(park);
		}
		
		public Task<IEnumerable<Park>> GetParks()
		{
            return Task.FromResult(_parkRepository.GetParks());
        }

        public Task<bool> AddPark(Park park)
        {
            // Validate that the park exists and it has been created with a name
            if (park == null)
                return Task.FromResult(false);
            if (string.IsNullOrWhiteSpace(park.Name))
                return Task.FromResult(false);

            // Wipe ID field (0 is default) so that the database can generate an ID automatically
            park.Id = 0;
			
            // Add the new park to the database
			_parkRepository.AddPark(park);
            _parkRepository.Save();

            return Task.FromResult(true);
		}
		
		public Task<bool> RemovePark(int parkId)
		{
            // Reject if ID is invalid
            if (parkId < 0)
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
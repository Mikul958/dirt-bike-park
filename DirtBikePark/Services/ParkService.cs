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

		private readonly DatabaseContext _context;
        public ParkService(DatabaseContext context)
        {
			_context = context;
        }

        public Task<Park?> GetPark(int parkId)
		{
            var park = _context.Parks
                .Include(p => p.Bookings)
                .FirstOrDefault(p => p.Id == parkId);
            return Task.FromResult(park);
		}
		
		public Task<IEnumerable<Park>> GetParks()
		{
            return Task.FromResult<IEnumerable<Park>>(_context.Parks.Include(p => p.Bookings));
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
			_context.Parks.Add(park);
            _context.SaveChanges();

            return Task.FromResult(true);
		}
		
		public Task<bool> RemovePark(int parkId)
		{
            // Reject if ID is invalid
            if (parkId < 0)
                return Task.FromResult(false);

            // Check if there is a park in the database with the given ID and return failure if not
            Park? park = _context.Parks
                .Include(park => park.Bookings)
                .FirstOrDefault(p => p.Id == parkId);
			if (park == null)
				return Task.FromResult(false);

            // Remove the park and associated bookings from the database
            _context.Bookings.RemoveRange(park.Bookings);
            _context.Parks.Remove(park);
			_context.SaveChanges();

			return Task.FromResult(true);
		}
	}
}
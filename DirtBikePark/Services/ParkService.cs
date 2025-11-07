using DirtBikePark.Data;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DirtBikePark.Services
{
	public class ParkService : IParkService
    {
		// written with mock data
		//private readonly List<Park> _parks = new List<Park>
		//{
		//	new Park { Id = 1, Name = "Park One", Description = "There are a lot of trees.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m },
		//	new Park { Id = 2, Name = "Park Two", Description = "There is a river through the middle.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m  },
		//	new Park { Id = 3, Name = "Park Three", Description = "It's pretty green.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m  }
		//};

		private readonly DatabaseContext _context;

        public ParkService(DatabaseContext context)
        {
			_context = context;
        }
        public Task<Park?> GetPark(int parkId)
		{
            //var park = _parks.Find(p => p.Id == parkId);
            var park = _context.Parks.Find(parkId);
            return Task.FromResult(park);
		}
		
		public Task<IEnumerable<Park>> GetParks()
		{
            //return Task.FromResult<IEnumerable<Park>>(_parks);
            return Task.FromResult<IEnumerable<Park>>(_context.Parks);

        }

        public Task<bool> AddPark(Park park)
        {
            if (park == null)
                return Task.FromResult(false);

            if (string.IsNullOrWhiteSpace(park.Name))
                return Task.FromResult(false);

            int assignedId;
            //if (park.Id > 0 && !_parks.Any(p => p.Id == park.Id))
            //	assignedId = park.Id;
            //else
            //	assignedId = _parks.Any() ? _parks.Max(p => p.Id) + 1 : 1;

            if (park.Id > 0 && !_context.Parks.Any(p => p.Id == park.Id))
                assignedId = park.Id;
            else
                assignedId = _context.Parks.Any() ? _context.Parks.Max(p => p.Id) + 1 : 1;

            park.Id = assignedId;
			//_parks.Add(park);
			_context.Parks.Add(park);
            _context.SaveChanges();

            return Task.FromResult(true);  // return success
		}
		
		public Task<bool> RemovePark(int parkId)
		{
			//var park = _parks.FirstOrDefault(p => p.Id == parkId);
			var park = _context.Parks.FirstOrDefault(p => p.Id == parkId);
			if (park == null)
				return Task.FromResult(false);
			//_parks.Remove(park);
			_context.Parks.Remove(park);
			_context.SaveChanges();
			return Task.FromResult(true);
		}
	}
}
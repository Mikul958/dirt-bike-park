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
		private readonly List<Park> _parks = new List<Park>
		{
			new Park { Id = 1, Name = "Park One", Location = "Columbia", Description = "There are a lot of trees.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m },
			new Park { Id = 2, Name = "Park Two", Location = "Charleston", Description = "There is a river through the middle.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m  },
			new Park { Id = 3, Name = "Park Three", Location = "Greenville", Description = "It's pretty green.", GuestLimit = 100, PricePerAdult = 25.00m, PricePerChild = 15.00m  }
		};
		public Task<Park?> GetPark(int parkId)
		{
			var park = _parks.Find(p => p.Id == parkId);
			return Task.FromResult(park);
		}
		
		public Task<IEnumerable<Park>> GetParks()
		{
			return Task.FromResult<IEnumerable<Park>>(_parks);
		}
		
		public Task<int> AddPark(Park park)
        {
            if (park == null)
                return Task.FromResult(false);

            if (string.IsNullOrWhiteSpace(park.Name) || string.IsNullOrWhiteSpace(park.Location))
                return Task.FromResult(false);

            int assignedId;
			if (park.Id > 0 && !_parks.Any(p => p.Id == park.Id))
				assignedId = park.Id;
			else
				assignedId = _parks.Any() ? _parks.Max(p => p.Id) + 1 : 1;

            park.Id = assignedId;
			_parks.Add(park);
			
			return Task.FromResult(assignedId); //returns Id of added park, might need to change? (?)
		}
		
		public Task<bool> RemovePark(int parkId)
		{
			var park = _parks.FirstOrDefault(p => p.Id == parkId);
			if (park == null)
				return Task.FromResult(false);
			_parks.Remove(park);
			return Task.FromResult(true);
		}
	}
}
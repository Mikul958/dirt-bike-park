using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DirtBikePark.Data
{
    public class ParkRepository : IParkRepository
    {
        private readonly DatabaseContext _context;
        public ParkRepository(DatabaseContext context)
        {
            _context = context;
        }
        public void AddPark(Park park)
        {
            _context.Parks.Add(park);
        }

        public Park? GetPark(int parkId)
        {
            var park = _context.Parks
                .FirstOrDefault(park => park.Id == parkId);
            return park;
        }

        public IEnumerable<Park> GetParks()
        {
            return _context.Parks;
        }

        public void RemovePark(Park park)
        {
            // Remove the park and associated bookings from the database
            _context.Bookings
                .Where(booking => booking.ParkId == park.Id)
                .ExecuteDelete();
            _context.Parks.Remove(park);
        }

        public void UpdatePark(Park park)
        {
           _context.Parks.Update(park);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

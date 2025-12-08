using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IParkRepository
    {
        Park? GetPark(int parkId);
        IEnumerable<Park> GetParks();
        void RemovePark(Park park);
        void AddPark(Park park);
        void UpdatePark(Park park);
        void Save();
    }
}

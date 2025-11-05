using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IParkService
    {
        Task<Park?> GetPark(int parkId);
        Task<IEnumerable<Park>> GetParks();
        Task<bool> RemovePark(int parkId);
        Task<int> AddPark(Park park);
    }
}
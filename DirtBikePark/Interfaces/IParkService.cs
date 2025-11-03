using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IParkService
    {
        Task<Park?> GetPark(Guid parkId);
        Task<IEnumerable<Park>> GetParks();
        Task<bool> RemovePark(Guid parkId);
    }
}
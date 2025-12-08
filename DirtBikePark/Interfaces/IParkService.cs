using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IParkService
    {
        Task<ParkResponseDTO> GetPark(int parkId);
        Task<IEnumerable<ParkResponseDTO>> GetParks();
        Task<ParkResponseDTO> AddPark(ParkInputDTO park);
        Task<bool> RemovePark(int parkId);
        Task<bool> EditPark(int parkId, ParkInputDTO newPark);
    }
}
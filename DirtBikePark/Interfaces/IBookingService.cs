using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponseDTO>> GetBookings();
        Task<IEnumerable<BookingResponseDTO>> GetParkBookings(int parkId);
        Task<BookingResponseDTO?> GetBooking(int bookingId);
        Task<BookingResponseDTO> CreateBooking(int parkId, BookingInputDTO booking);
        Task<bool> RemoveBooking(int bookingId);
    }
}


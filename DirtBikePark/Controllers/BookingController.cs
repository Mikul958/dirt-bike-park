using DirtBikePark.Attributes;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.AspNetCore.Mvc;

namespace DirtBikePark.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET /api/booking
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            IEnumerable<BookingResponseDTO> items = await _bookingService.GetBookings();
            return Ok(items);
        }

        // GET /api/booking/park/{parkId}
        [HttpGet("park/{parkId:int}")]
        public async Task<IActionResult> GetParkBookings([PositiveId][FromRoute] int parkId)
        {
            IEnumerable<BookingResponseDTO> items = await _bookingService.GetParkBookings(parkId);
            return Ok(items);
        }

        // GET /api/booking/{bookingId}
        [HttpGet("{bookingId:int}")]
        public async Task<IActionResult> GetBooking([PositiveId][FromRoute] int bookingId)
        {
            BookingResponseDTO? booking = await _bookingService.GetBooking(bookingId);
            if (booking != null)
                return Ok(booking);
            else
                return NotFound($"No booking found with ID {bookingId}.");
        }

        // POST /api/booking/park/{parkId}
        [HttpPost("park/{parkId:int}")]
        public async Task<IActionResult> CreateBooking([PositiveId][FromRoute] int parkId, [FromBody] BookingInputDTO bookingInfo)
        {
            try
            {
                bool created = await _bookingService.CreateBooking(parkId, bookingInfo);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/booking/{bookingId}
        [HttpDelete("{bookingId:int}")]
        public async Task<IActionResult> RemoveBooking([PositiveId][FromRoute] int bookingId)
        {
            try
            {
                bool ok = await _bookingService.RemoveBooking(bookingId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}


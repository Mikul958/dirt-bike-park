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
        public async Task<ActionResult<IReadOnlyList<Booking>>> GetBookings()
        {
            var items = await _bookingService.GetBookingsAsync();
            return Ok(items);
        }

        // GET /api/booking/park/{parkId}
        // (Your "GetBooking(parkId)" that returns a LIST)
        [HttpGet("park/{parkId:int}")]
        public async Task<ActionResult<IReadOnlyList<Booking>>> GetBooking([FromRoute] int parkId)
        {
            var items = await _bookingService.GetBookingsByParkAsync(parkId);
            return Ok(items);
        }

        // POST /api/booking/park/{parkId}
        [HttpPost("park/{parkId:int}")]
        public async Task<ActionResult<Booking>> CreateBooking([FromRoute] int parkId, [FromBody] Booking booking)
        {
            // Ignore incoming Id; server assigns
            booking.Id = 0;

            var created = await _bookingService.CreateBookingAsync(parkId, booking);

            return CreatedAtAction(
                nameof(GetBooking),
                new { parkId = parkId },
                created
            );
        }

        // DELETE /api/booking/{bookingId}
        [HttpDelete("{bookingId:int}")]
        public async Task<IActionResult> RemoveBooking([FromRoute] int bookingId)
        {
            var ok = await _bookingService.RemoveBookingAsync(bookingId);
            return ok ? NoContent() : NotFound();
        }
    }
}


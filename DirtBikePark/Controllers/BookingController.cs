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

        /// <summary>Get all bookings in a cart.</summary>
        [HttpGet("{cartId:guid}")]
        public async Task<ActionResult<IReadOnlyList<Booking>>> GetBookings([FromRoute] Guid cartId)
        {
            var items = await _bookingService.GetBookingsAsync(cartId);
            return Ok(items);
        }

        /// <summary>Get a single booking by id.</summary>
        [HttpGet("{cartId:guid}/{bookingId:int}")]
        public async Task<ActionResult<Booking>> GetBooking([FromRoute] Guid cartId, [FromRoute] int bookingId)
        {
            var booking = await _bookingService.GetBookingAsync(cartId, bookingId);
            if (booking is null) return NotFound();
            return Ok(booking);
        }

        /// <summary>Create a booking in a cart.</summary>
        [HttpPost("{cartId:guid}")]
        public async Task<ActionResult<Booking>> CreateBooking([FromRoute] Guid cartId, [FromBody] Booking booking)
        {
            // Defensive: ignore incoming Id/CartId and set server-side
            booking.Id = 0;
            booking.CartId = cartId;

            var created = await _bookingService.CreateBookingAsync(cartId, booking);
            return CreatedAtAction(nameof(GetBooking),
                new { cartId = cartId, bookingId = created.Id }, created);
        }

        /// <summary>Remove a booking from a cart.</summary>
        [HttpDelete("{cartId:guid}/{bookingId:int}")]
        public async Task<IActionResult> RemoveBooking([FromRoute] Guid cartId, [FromRoute] int bookingId)
        {
            var ok = await _bookingService.RemoveBookingAsync(cartId, bookingId);
            return ok ? NoContent() : NotFound();
        }
    }
}

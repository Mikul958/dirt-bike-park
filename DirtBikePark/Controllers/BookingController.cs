using DirtBikePark.Attributes;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Get All Bookings")]
        public async Task<IActionResult> GetBookings()
        {
            IEnumerable<BookingResponseDTO> items = await _bookingService.GetBookings();
            return Ok(items);
        }

        // GET /api/booking/park/{parkId}
        [HttpGet("park/{parkId:int}")]
        [SwaggerOperation(Summary = "Get Bookings By Park")]
        public async Task<IActionResult> GetParkBookings([PositiveId][FromRoute] int parkId)
        {
            IEnumerable<BookingResponseDTO> items = await _bookingService.GetParkBookings(parkId);
            return Ok(items);
        }

        // GET /api/booking/{bookingId}
        [HttpGet("{bookingId:int}")]
        [SwaggerOperation(Summary = "Get Booking")]
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
        [SwaggerOperation(Summary = "Create Booking")]
        public async Task<IActionResult> CreateBooking([PositiveId][FromRoute] int parkId, [FromBody] BookingInputDTO bookingInfo)
        {
            try
            {
                BookingResponseDTO createdBooking = await _bookingService.CreateBooking(parkId, bookingInfo);

                // Return 201 created with route associated with GetBooking + supplied bookingId and a corresponding BookingResponseDTO in the body
                return CreatedAtAction(nameof(GetBooking), new { bookingId = createdBooking.Id }, createdBooking);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/booking/{bookingId}
        [HttpDelete("{bookingId:int}")]
        [SwaggerOperation(Summary = "Remove Booking")]
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


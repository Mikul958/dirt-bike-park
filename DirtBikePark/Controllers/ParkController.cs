using DirtBikePark.Attributes;
using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DirtBikePark.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkController : ControllerBase
    {
        private readonly IParkService _parkService;

        public ParkController(IParkService parkService)
        {
            _parkService = parkService;
        }

        // GET {protocol}://{urlBase}/api/park/{parkId}
        [HttpGet("{parkId:int}")]
        public async Task<IActionResult> GetPark([PositiveId][FromRoute] int parkId)
        {
            try
            {
                ParkResponseDTO? park = await _parkService.GetPark(parkId);
                return Ok(park);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET {protocol}://{urlBase}/api/park/
        [HttpGet]
        public async Task<IActionResult> GetParks()
        {
            try
            {
                IEnumerable<ParkResponseDTO> parks = await _parkService.GetParks();
                return Ok(parks);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST {protocol}://{urlBase}/api/park/
        [HttpPost]
        public async Task<IActionResult> AddPark([FromBody] ParkInputDTO park)
        {
            if (park == null)
                return BadRequest("Park can not be null.");
            try
            {
                ParkResponseDTO createdPark = await _parkService.AddPark(park);

                // Return 201 created with route associated with GetBooking + supplied bookingId and a corresponding BookingResponseDTO in the body
                return CreatedAtAction(nameof(GetPark), new { parkId = createdPark.Id }, createdPark);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE {protocol}://{urlBase}/api/park/{parkId}
        [HttpDelete("{parkId:int}")]
        public async Task<IActionResult> RemovePark([PositiveId][FromRoute] int parkId)
        {
            try
            {
                bool removed = await _parkService.RemovePark(parkId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST {protocol}://{urlBase}/api/park/{parkId}/guestlimit?guestLimit={guestLimit}
        [HttpPost("{parkId}/guestlimit")]
        public async Task<IActionResult> AddGuestLimitToPark([FromRoute] int parkId, [FromQuery] int guestLimit)
        {
            // Validate that the parkId is valid.
            if (parkId <= 0)
                return BadRequest("Park id is invalid.");
            // Validate that the number of guests is valid.
            if (guestLimit <= 0)
                return BadRequest("Number of guests must be greater than zero.");

            try
            {
                // Call service
                bool success = await _parkService.AddGuestLimitToPark(parkId, guestLimit);
                return Ok(success);
            }
            catch (InvalidOperationException ex)
            {
                // Handles invalid arguments
                return BadRequest(ex.Message);
            }
        }

        // DELETE {protocol}://{urlBase}/api/park/{parkId}/removeGuests?date={date}&numberOfGuests={numberOfGuests}
        [HttpDelete("{parkId:int}/removeGuests")]
        public async Task<IActionResult> RemoveGuestsFromPark([PositiveId][FromRoute] int parkId, [FromQuery] DateOnly date, [FromQuery] int numberofGuests)
        {
            if (numberofGuests < 1)
                return BadRequest("Number of guests to remove cannot be less than 1.");
            
            try
            {
                bool guestsRemoved = await _parkService.RemoveGuestsFromPark(parkId, date, numberofGuests);
                return Ok(guestsRemoved);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
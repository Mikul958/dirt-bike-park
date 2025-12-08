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
            catch(InvalidOperationException ex)
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
                bool success = await _parkService.AddPark(park);  // Note: removing returned object for now since project just says to return success or failure
                return Ok(success);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

		}

        // POST {protocol}://{urlBase}/api/park/{parkId}/guestlimit
        [HttpPost("{parkId}/guestlimit")]
        public async Task<IActionResult> AddGuestLimitToPark([FromRoute] int parkId, [FromBody] int guestLimit)
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
            catch (ArgumentException ex)
            {
                // Handles invalid arguments
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                // Handles park not found
                return NotFound(ex.Message);
            }
        }

        // DELETE {protocol}://{urlBase}/api/park/{parkId}
        [HttpDelete("{parkId:int}")]
        public async Task<IActionResult> RemovePark([PositiveId][FromRoute] int parkId)
        {
            try
            {
                bool removed = await _parkService.RemovePark(parkId);
                return Ok(removed);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
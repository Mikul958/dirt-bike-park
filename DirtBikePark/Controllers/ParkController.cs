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
        public async Task<IActionResult> GetPark([FromRoute] int parkId)
        {
            ParkResponseDTO? park = await _parkService.GetPark(parkId);
            if (park == null)
                return NotFound($"Park with ID {parkId} not found.");
            return Ok(park);
        }

        // GET {protocol}://{urlBase}/api/park/
        [HttpGet]
        public async Task<IActionResult> GetParks()
        {
            IEnumerable<ParkResponseDTO> parks = await _parkService.GetParks();
            return Ok(parks);
        }
		
		// POST {protocol}://{urlBase}/api/park/
		[HttpPost]
		public async Task<IActionResult> AddPark([FromBody] ParkInputDTO park)
		{
            if (park == null)
                return BadRequest("Park can not be null.");

            bool success = await _parkService.AddPark(park);  // Note: removing returned object for now since project just says to return success or failure
			return Ok(success);
		}

        // DELETE {protocol}://{urlBase}/api/park/{parkId}
        [HttpDelete("{parkId:int}")]
        public async Task<IActionResult> RemovePark([FromRoute] int parkId)
        {
            bool removed = await _parkService.RemovePark(parkId);
            if (removed)
                return NoContent();
            return NotFound($"Park with ID {parkId} was not found.");
        }
    }
}
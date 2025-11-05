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
            var park = await _parkService.GetPark(parkId);
            if (park == null)
            {
                return NotFound($"Park with ID {parkId} not found.");
            }
            return Ok(park);
        }

        // GET {protocol}://{urlBase}/api/park/
        [HttpGet]
        public async Task<IActionResult> GetParks()
        {
            var parks = await _parkService.GetParks();
            return Ok(parks);
        }

        // DELETE {protocol}://{urlBase}/api/park/{parkId}
        [HttpDelete("{parkId:int}")]
        public async Task<IActionResult> RemovePark([FromRoute] int parkId)
        {
            var removed = await _parkService.RemovePark(parkId);
            if (removed)
            {
                return NoContent();
            }
            return NotFound($"Park with ID {parkId} was not found.");
        }
		
		// POST {protocol}://{urlBase}/api/park/
		[HttpPost]
		public async Task<IActionResult> AddPark([FromBody] Park park)
		{
            if (park == null)
                return BadRequest("Park can not be null.");

            int newId = await _parkService.AddPark(park);
            if (newId <= 0)
                return BadRequest("Invalid park data.");

			return CreatedAtAction(nameof(GetPark), new { parkId = newId }, park);
		}
    }
}
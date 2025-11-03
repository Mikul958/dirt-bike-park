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
        [HttpGet("{parkId:guid}")]
        public async Task<IActionResult> GetPark([FromRoute] Guid parkId)
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
        [HttpDelete("{parkId:guid}")]
        public async Task<IActionResult> RemovePark([FromRoute] Guid parkId)
        {
            var removed = await _parkService.RemovePark(parkId);
            if (removed)
            {
                return NoContent();
            }
            return NotFound($"Park with ID {parkId} was not found.");
        }
    }
}

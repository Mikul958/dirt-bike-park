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
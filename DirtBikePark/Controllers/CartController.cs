using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DirtBikePark.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET {protocol}://{urlBase}/api/cart/{cartId}
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCart([FromRoute] string cartId)
        {
            Guid processedCartId;
            if (!Guid.TryParse(cartId, out processedCartId))
                return BadRequest("Could not find a cart with the provided cart ID");
            Cart cart = await _cartService.GetCart(processedCartId);
            return Ok(cart);
        }

        // POST {protocol}://{urlBase}/api/cart/{cartId}/add?parkId={parkId} -- bookingInfo sent in request body
        [HttpPost("{cartId}/add")]
        public async Task<IActionResult> AddBookingToCart([FromRoute] string cartId, [FromQuery] int parkId, [FromQuery] int bookingId)
        {
            Guid processedCartId;
            if (!Guid.TryParse(cartId, out processedCartId))
                return BadRequest("Could not find a cart with the provided cart ID");
            bool addStatus = await _cartService.AddBookingToCart(processedCartId, parkId, bookingId);
            return Ok(addStatus);
        }

        // PUT {protocol}://{urlBase}/api/cart/{cardId}/remove?bookingId={bookingId}
        [HttpPut("{cartId}/remove")]
        public async Task<IActionResult> RemoveBookingFromCart([FromRoute] string cartId, [FromQuery] int bookingId)
        {
            Guid processedCartId;
            if (!Guid.TryParse(cartId, out processedCartId))
                return BadRequest("Could not find a cart with the provided cart ID");
            bool removeStatus = await _cartService.RemoveBookingFromCart(processedCartId, bookingId);
            return Ok(removeStatus);
        }
    }
}

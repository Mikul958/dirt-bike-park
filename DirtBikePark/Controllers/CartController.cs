using DirtBikePark.Attributes;
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
        [HttpGet("{cartId?}")]
        public async Task<IActionResult> GetCart([FromRoute] string? cartId)
        {
            // Verify that the provided Guid is valid (if it isn't null)
            Guid parsedCartId = Guid.Empty;
            Guid? finalCartId = null;
            if (cartId != null && Guid.TryParse(cartId, out parsedCartId))
                finalCartId = parsedCartId;
            else
                finalCartId = null;

            // Retrieve cart through service
            CartResponseDTO cart = await _cartService.GetCart(finalCartId);
            return Ok(cart);
        }

        // POST {protocol}://{urlBase}/api/cart/{cartId}/add?parkId={parkId} -- bookingInfo sent in request body
        [HttpPost("{cartId}/add")]
        public async Task<IActionResult> AddBookingToCart([FromRoute] string cartId, [PositiveId][FromQuery] int parkId, [PositiveId][FromQuery] int bookingId)
        {
            // Verify that the provided cartId is a valid Guid
            Guid processedCartId;
            if (!Guid.TryParse(cartId, out processedCartId))
                return BadRequest("Could not find a cart with the provided cart ID");

            try
            {
                // Link booking to cart through service
                bool addStatus = await _cartService.AddBookingToCart(processedCartId, parkId, bookingId);
                return Ok(addStatus);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // PUT {protocol}://{urlBase}/api/cart/{cardId}/remove?bookingId={bookingId}
        [HttpPut("{cartId}/remove")]
        public async Task<IActionResult> RemoveBookingFromCart([FromRoute] string cartId, [PositiveId][FromQuery] int bookingId)
        {
            // Verify that the provided cartId is a valid Guid
            Guid processedCartId;
            if (!Guid.TryParse(cartId, out processedCartId))
                return BadRequest("Could not find a cart with the provided cart ID");

            try
            {
                // Remove link between booking and cart through service
                bool removeStatus = await _cartService.RemoveBookingFromCart(processedCartId, bookingId);
                return Ok(removeStatus);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST {protocol}://{urlBase}/api/cart/{cartId}/payment?cardNumber={cardNumber}&exp={exp}&cardHolderName={cardHolderName}&ccv={ccv}
        [HttpPost("{cartId}/payment")]
        public async Task<IActionResult> ProcessPayment([FromRoute] string cartId, [FromQuery] string cardNumber, [FromQuery] DateOnly exp, [FromQuery] string cardHolderName, [FromQuery] string ccv)
        {
            // Verify that the provided cartId is a valid Guid
            Guid processedCartId;
            if (!Guid.TryParse(cartId, out processedCartId))
                return BadRequest("Could not find a cart with the provded cart ID");

            // Pack card info into a Payment object and call service
            PaymentInfo paymentInfo = new PaymentInfo(cardNumber, ccv, cardHolderName, exp);
            try
            {
                bool paymentSuccess = await _cartService.ProcessPayment(processedCartId, paymentInfo);
                return Ok(paymentSuccess);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

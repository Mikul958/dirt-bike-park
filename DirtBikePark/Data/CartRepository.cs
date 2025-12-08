using DirtBikePark.Interfaces;
using DirtBikePark.Models;
using Microsoft.EntityFrameworkCore;

namespace DirtBikePark.Data
{
    public class CartRepository : ICartRepository
    {
        private readonly DatabaseContext _context;

        public CartRepository(DatabaseContext context)
        {
            _context = context;
        }

        public Cart? GetCart(Guid? cartId)
        {
            return _context.Carts
                .Include(cart => cart.Bookings
                    .Where(booking => !booking.IsPaidFor))  // Filter bookings to bookings that have not been paid for / finalized
                .ThenInclude(booking => booking.Park)
                .FirstOrDefault(cart => cart.Id == cartId);
        }

        public void AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
        }

        public void FinalizePayment(Cart cart)
        {
            foreach (Booking booking in cart.Bookings)
                booking.IsPaidFor = true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

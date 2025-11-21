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
        public void AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
        }

        public Cart? GetCart(Guid? cartId)
        {
            return _context.Carts
                .Include(cart => cart.Bookings)
                .ThenInclude(booking => booking.Park)
                .FirstOrDefault(cart => cart.Id == cartId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

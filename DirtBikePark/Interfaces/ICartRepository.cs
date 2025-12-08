using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface ICartRepository
    {
        Cart? GetCart(Guid? cartId);
        void AddCart(Cart cart);
        void FinalizePayment(Cart cart);
        void Save();
    }
}

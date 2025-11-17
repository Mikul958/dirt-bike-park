using DirtBikePark.Models;

namespace DirtBikePark.Interfaces
{
    public interface ICartRepository
    {
        Cart? GetCart(Guid? cartId);
        void AddCart(Cart cart);
        //void RemoveCart(Guid cartId);
        void Save();
    }
}

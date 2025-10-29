using System.ComponentModel.DataAnnotations;

namespace DirtBikePark.Models
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }
        public List<Booking> Bookings { get; set; }

        public Cart()
        {
            // TODO
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DirtBikePark.Models
{
    public class Park
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Booking> Bookings { get; set; }
        public int GuestLimit { get; set; }
        public decimal PricePerAdult { get; set; }
        public decimal PricePerChild { get; set; }

        public Park()
        {
            // TODO
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirtBikePark.Models
{
    public class Park
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public int GuestLimit { get; set; }
        public decimal PricePerAdult { get; set; }
        public decimal PricePerChild { get; set; }
    }
}

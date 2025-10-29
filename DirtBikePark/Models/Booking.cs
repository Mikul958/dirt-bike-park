using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirtBikePark.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Cart")]
        public Guid CartID { get; set; }
        [ForeignKey("Park")]
        public int ParkID { get; set; }
        public string Date { get; set; } = string.Empty;
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

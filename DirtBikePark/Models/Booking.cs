using Microsoft.EntityFrameworkCore;
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
        public Guid? CartId { get; set; }

        [ForeignKey("Park")]
        public int ParkId { get; set; }
        public Park? Park { get; set; }

        // public int NumDays { get; set; }
        public DateOnly Date { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        [Precision(18,2)]
        public decimal TotalPrice { get; set; }
        public bool IsPaidFor { get; set; }
    }
}

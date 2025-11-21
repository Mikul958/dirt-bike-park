using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirtBikePark.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Cart")]
        public Guid CartId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string Ccv { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public string HolderName { get; set; } = string.Empty;
    }
}

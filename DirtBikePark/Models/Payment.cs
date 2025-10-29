using System.ComponentModel.DataAnnotations.Schema;

namespace DirtBikePark.Models
{
    public class Payment
    {
        [ForeignKey("Cart")]
        public Guid CartID { get; set; }
        public string CardNumber { get; set; }
        public string Ccv { get; set; }
        public string ExpirationDate { get; set; }
        public string HolderName { get; set; }
        public string BillingAddress { get; set; }

        public Payment()
        {
            // TODO
        }
    }
}

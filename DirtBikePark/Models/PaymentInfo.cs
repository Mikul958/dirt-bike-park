using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirtBikePark.Models
{
    public class PaymentInfo
    {
        public string CardNumber { get; set; } = string.Empty;
        public string Ccv { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public DateOnly ExpirationDate { get; set; }

        public PaymentInfo(string cardNumber, string ccv, string cardHolderName, DateOnly expirationDate)
        {
            CardNumber = cardNumber;
            Ccv = ccv;
            CardHolderName = cardHolderName;
            ExpirationDate = expirationDate;
        }
    }
}

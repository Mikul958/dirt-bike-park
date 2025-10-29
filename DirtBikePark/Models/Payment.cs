namespace DirtBikePark.Models
{
    public class Payment
    {
        public string cartID { get; set; }
        public string cardNumber { get; set; }
        public string ccv { get; set; }
        public Date expiration { get; set; }
        public String holderName { get; set; }
        public string billingAddress { get; set; }

        public Payment()
        {
            // TODO
        }
	}
}
